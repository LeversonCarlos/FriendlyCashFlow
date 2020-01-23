import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../dashboard.service';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';
import { enCategoryType } from '../../categories/categories.service';

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
         const accountBalances = await this.dashboardService.getBalances();

         let income = Object.assign(new ResumeVM, { Text: 'Income', Value: 10, Type: enCategoryType.Income, Icon: 'add_circle' });
         let expense = Object.assign(new ResumeVM, { Text: 'Expense', Value: 3, Type: enCategoryType.Expense, Icon: 'remove_circle' });
         let balance = Object.assign(new ResumeVM, { Text: 'Balance', Value: 7, Type: enCategoryType.None, Icon: 'monetization_on' });

         this.ResumeList = [income, expense, balance]
      }
      catch (ex) { this.appInsights.trackException(ex) }
   }

}
