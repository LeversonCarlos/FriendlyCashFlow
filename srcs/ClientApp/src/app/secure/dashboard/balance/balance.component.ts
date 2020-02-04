import { Component, OnInit } from '@angular/core';
import { Balance } from '../dashboard.viewmodels';
import { DashboardService } from '../dashboard.service';
import { AccountsService } from '../../accounts/accounts.service';
import { enAccountType } from '../../accounts/accounts.viewmodels';
import { BehaviorSubject } from 'rxjs';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';

class BalanceTypeVM {
   Type: enAccountType
   Text: string
   Icon: string
   TotalValue: number
   PaidValue: number
   Accounts: Balance[]
}

@Component({
   selector: 'fs-balance',
   templateUrl: './balance.component.html',
   styleUrls: ['./balance.component.scss']
})
export class BalanceComponent implements OnInit {

   constructor(private accountsService: AccountsService, private dashboardService: DashboardService, private appInsights: AppInsightsService) { }

   private Key: string = 'DASHBOARD_BALANCE_DATA'
   public Data = new BehaviorSubject<BalanceTypeVM[]>([])

   public async ngOnInit() {
      this.cacheLoad()
      const data = await this.getRefreshedData()
      this.cacheSave(data)
   }

   private cacheLoad() {
      try {
         const jsonData = localStorage.getItem(this.Key)
         if (jsonData && jsonData != '') {
            const data: BalanceTypeVM[] = JSON.parse(jsonData)
            if (data) {
               this.Data.next(data)
            }
         }
      }
      catch (ex) { this.appInsights.trackException(ex) }
   }

   private async getRefreshedData(): Promise<BalanceTypeVM[]> {
      try {
         const accountTypes = await this.accountsService.getAccountTypes();
         const accountBalances = await this.dashboardService.getBalances();

         return accountTypes
            .map(accountType => {

               let balanceType = new BalanceTypeVM()
               balanceType.Type = accountType.Value
               balanceType.Text = accountType.Text
               balanceType.Icon = accountType.Icon

               balanceType.Accounts = accountBalances
                  .filter(accountBalance => accountBalance.Type == balanceType.Type)
                  .sort((a, b) => a.Text < b.Text ? -1 : 1)

               if (balanceType.Type == enAccountType.CreditCard && balanceType.Accounts) {
                  balanceType.Accounts.forEach(account => {
                     account.CurrentBalance += account.IncomeForecast
                     account.CurrentBalance += account.ExpenseForecast
                     account.IncomeForecast = 0
                     account.ExpenseForecast = 0
                  })
               }

               balanceType.TotalValue = 0
               balanceType.PaidValue = 0
               if (balanceType.Accounts && balanceType.Accounts.length > 0) {
                  balanceType.PaidValue = balanceType.Accounts
                     .map(x => x.CurrentBalance)
                     .reduce((p, n) => p + n, 0)
                  balanceType.TotalValue = balanceType.PaidValue +
                     balanceType.Accounts
                        .map(x => x.IncomeForecast + x.ExpenseForecast)
                        .reduce((p, n) => p + n, 0)

               }

               return balanceType
            })
            .filter(balanceType => balanceType.Accounts && balanceType.Accounts.length > 0)
            .sort((a, b) => a.Type < b.Type ? -1 : 1)

      }
      catch (ex) { this.appInsights.trackException(ex); return null; }
   }

   private async cacheSave(data: BalanceTypeVM[]) {
      try {
         this.Data.next(data)
         const jsonData = JSON.stringify(data);
         localStorage.setItem(this.Key, jsonData)
      }
      catch (ex) { this.appInsights.trackException(ex) }
   }

   public GetUrl(account: Balance): string {
      const now = new Date()
      const year = now.getFullYear()
      const month = now.getMonth() + 1
      return `/entries/flow/${year}/${month}/${account.AccountID}`
   }

}
