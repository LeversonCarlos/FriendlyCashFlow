import { Component, OnInit, Input } from '@angular/core';
import { ResumeVM } from './resume.viewmodels';
import { enCategoryType } from '../../categories/categories.viewmodels';


@Component({
   selector: 'fs-common-resume',
   templateUrl: './resume.component.html',
   styleUrls: ['./resume.component.scss']
})
export class CommonResumeComponent implements OnInit {

   constructor() { }

   private IncomeResume: ResumeVM = Object.assign(new ResumeVM, { Text: 'DASHBOARD_RESUME_INCOME_TEXT', Value: 0, Type: enCategoryType.Income, Icon: 'add_circle' });
   @Input() public set IncomeValue(val: number) { this.IncomeResume.Value = val };

   private ExpenseResume: ResumeVM = Object.assign(new ResumeVM, { Text: 'DASHBOARD_RESUME_EXPENSE_TEXT', Value: 0, Type: enCategoryType.Expense, Icon: 'remove_circle' });
   @Input() public set ExpenseValue(val: number) { this.ExpenseResume.Value = val };

   private BalanceResume: ResumeVM = Object.assign(new ResumeVM, { Text: 'DASHBOARD_RESUME_BALANCE_TEXT', Value: 0, Type: enCategoryType.None, Icon: 'monetization_on' });
   @Input() public set BalanceValue(val: number) { this.BalanceResume.Value = val };

   public ResumeList: ResumeVM[] = [this.IncomeResume, this.ExpenseResume, this.BalanceResume];

   public ngOnInit() {
   }

}
