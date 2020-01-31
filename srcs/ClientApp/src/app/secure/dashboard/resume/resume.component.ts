import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../dashboard.service';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';

@Component({
   selector: 'fs-resume',
   templateUrl: './resume.component.html',
   styleUrls: ['./resume.component.scss']
})
export class ResumeComponent implements OnInit {

   constructor(private dashboardService: DashboardService, private appInsights: AppInsightsService) { }

   public IncomeValue: number = 0;
   public ExpenseValue: number = 0;
   public BalanceValue: number = 0;

   public async ngOnInit() {
      try {

         let accountBalances = await this.dashboardService.getBalances(true);
         if (accountBalances) {
            for (let index = 0; index < accountBalances.length; index++) {
               const account = accountBalances[index];
               const incomeValue = (account.PaidIncome + account.IncomeForecast);
               const expenseValue = (account.PaidExpense + account.ExpenseForecast);
               this.IncomeValue += incomeValue
               this.ExpenseValue += expenseValue
               this.BalanceValue += incomeValue
               this.BalanceValue += expenseValue
            }
         }

      }
      catch (ex) { this.appInsights.trackException(ex) }
   }

}
