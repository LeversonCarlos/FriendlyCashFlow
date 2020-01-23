import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../dashboard.service';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';
import { enCategoryType } from '../../categories/categories.service';
import { enAccountType } from '../../accounts/accounts.viewmodels';

class ResumeVM {
   Text: string
   Type: enCategoryType
   Icon: string
   Value: number
}

@Component({
   selector: 'fs-resume',
   templateUrl: './resume.component.html',
   styleUrls: ['./resume.component.scss']
})
export class ResumeComponent implements OnInit {

   constructor(private dashboardService: DashboardService,
      private appInsights: AppInsightsService) { }
   public ResumeList: ResumeVM[]

   public async ngOnInit() {
      try {

         let income = Object.assign(new ResumeVM, { Text: 'DASHBOARD_RESUME_INCOME_TEXT', Value: 10, Type: enCategoryType.Income, Icon: 'add_circle' });
         let expense = Object.assign(new ResumeVM, { Text: 'DASHBOARD_RESUME_EXPENSE_TEXT', Value: 3, Type: enCategoryType.Expense, Icon: 'remove_circle' });
         let balance = Object.assign(new ResumeVM, { Text: 'DASHBOARD_RESUME_BALANCE_TEXT', Value: 7, Type: enCategoryType.None, Icon: 'monetization_on' });

         let accountBalances = await this.dashboardService.getBalances();
         if (accountBalances) {
            for (let index = 0; index < accountBalances.length; index++) {
               const account = accountBalances[index];
               income.Value += account.IncomeForecast
               expense.Value += account.ExpenseForecast
               balance.Value += account.IncomeForecast
               balance.Value += account.ExpenseForecast
            }
         }

         this.ResumeList = [income, expense, balance]
      }
      catch (ex) { this.appInsights.trackException(ex) }
   }

}
