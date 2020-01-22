import { Component, OnInit } from '@angular/core';
import { Balance } from '../dashboard.viewmodels';
import { DashboardService } from '../dashboard.service';
import { AccountsService } from '../../accounts/accounts.service';
import { enAccountType, Account } from '../../accounts/accounts.viewmodels';
import { EntriesService } from '../../entries/entries.service';

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

   constructor(private accountsService: AccountsService, private dashboardService: DashboardService, private entriesService: EntriesService) { }

   public balanceTypes: BalanceTypeVM[];

   public async ngOnInit() {
      const accountTypes = await this.accountsService.getAccountTypes();
      const accountBalances = await this.dashboardService.getBalances();

      this.balanceTypes = accountTypes
         .map(accountType => {

            let balanceType = new BalanceTypeVM()
            balanceType.Type = accountType.Value
            balanceType.Text = accountType.Text
            balanceType.Icon = accountType.Icon

            balanceType.Accounts = accountBalances
               .filter(accountBalance => accountBalance.Type == balanceType.Type)
               .sort((a, b) => a.Text < b.Text ? -1 : 1)

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

   public OnClick(account: Balance) {
      const now = new Date()
      const year = now.getFullYear()
      const month = now.getMonth() + 1
      this.entriesService.showFlow(year, month, account.AccountID)
   }

}
