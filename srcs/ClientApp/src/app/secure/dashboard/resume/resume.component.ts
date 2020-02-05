import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../dashboard.service';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';
import { BehaviorSubject } from 'rxjs';

class DashboardResumeVM {
   IncomeValue: number = 0;
   ExpenseValue: number = 0;
   BalanceValue: number = 0;
}

@Component({
   selector: 'fs-resume',
   templateUrl: './resume.component.html',
   styleUrls: ['./resume.component.scss']
})
export class ResumeComponent implements OnInit {

   constructor(private dashboardService: DashboardService, private appInsights: AppInsightsService) { }

   private Key: string = 'DASHBOARD_RESUME_DATA'
   public Data = new BehaviorSubject<DashboardResumeVM>(new DashboardResumeVM())

   public async ngOnInit() {
      this.cacheLoad()
      const data = await this.getRefreshedData()
      this.cacheSave(data)
   }

   private cacheLoad() {
      try {
         const jsonData = localStorage.getItem(this.Key)
         if (jsonData && jsonData != '') {
            const data: DashboardResumeVM = JSON.parse(jsonData)
            if (data) {
               this.Data.next(data)
            }
         }
      }
      catch (ex) { this.appInsights.trackException(ex) }
   }

   private async getRefreshedData(): Promise<DashboardResumeVM> {
      try {
         let data = new DashboardResumeVM()

         let accountBalances = await this.dashboardService.getBalances(true);
         if (accountBalances) {
            for (let index = 0; index < accountBalances.length; index++) {
               const account = accountBalances[index];
               const incomeValue = (account.PaidIncome + account.IncomeForecast);
               const expenseValue = (account.PaidExpense + account.ExpenseForecast);
               data.IncomeValue += incomeValue
               data.ExpenseValue += expenseValue
               data.BalanceValue += incomeValue
               data.BalanceValue += expenseValue
            }
         }

         return data;
      }
      catch (ex) { this.appInsights.trackException(ex); return null; }
   }

   private async cacheSave(data: DashboardResumeVM) {
      try {
         this.Data.next(data)
         const jsonData = JSON.stringify(data);
         localStorage.setItem(this.Key, jsonData)
      }
      catch (ex) { this.appInsights.trackException(ex) }
   }

}
