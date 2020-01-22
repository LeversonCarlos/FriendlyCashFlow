import { Component, OnInit } from '@angular/core';
import { Balance } from '../dashboard.viewmodels';
import { DashboardService } from '../dashboard.service';
import { AccountsService } from '../../accounts/accounts.service';
import { enAccountType } from '../../accounts/accounts.viewmodels';

class BalanceTypeVM {
   Type: enAccountType
   Text: string
   Accounts: Balance[]
}

@Component({
   selector: 'fs-balance',
   templateUrl: './balance.component.html',
   styleUrls: ['./balance.component.scss']
})
export class BalanceComponent implements OnInit {

   constructor(private accountsService: AccountsService, private dashboardService: DashboardService) { }

   public balanceTypes: BalanceTypeVM[];

   public async ngOnInit() {
      const accountTypes = await this.accountsService.getAccountTypes();
      const accountBalances = await this.dashboardService.getBalances();

      this.balanceTypes = accountTypes
         .map(accountType => {
            let balanceType = new BalanceTypeVM()
            balanceType.Type = accountType.Value
            balanceType.Text = accountType.Text
            balanceType.Accounts = accountBalances.filter(accountBalance => accountBalance.Type == balanceType.Type)
            return balanceType
         })
         .filter(balanceType => balanceType.Accounts && balanceType.Accounts.length > 0)
         .sort((a, b) => a.Type < b.Type ? -1 : 1)
   }

}
