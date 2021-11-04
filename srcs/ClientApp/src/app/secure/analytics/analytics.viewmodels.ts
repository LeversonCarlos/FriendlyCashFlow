export class FilterData {
   public Month: Date

   public setData(month: Date) {
      this.Month = month
   }

}

export class CategoryGoalsVM {
   CategoryID: number
   Text: string
   ParentID: number
   CategoryValue: number
   CategoryPercent: number
   AverageValue: number
   AveragePercent: number
   Childs: CategoryGoalsVM[]
}

export class EntriesParetoVM {
   Text: string
   CategoryID: number
   Value: number
   Pareto: number
}

export class MonthlyTargetVM {
   SearchDate: Date
   SmallText: string
   FullText: string
   IncomeValue: number
   IncomeAverage: number
   IncomeTarget: number
   ExpenseValue: number
   ExpenseAverage: number
   ExpenseTarget: number
   Balance: number
}

export class MonthlyBudgetVM {
   PatternID: number
   Text: string
   CategoryID: number
   OverflowValue: number
   OverflowPercent: number
}

export class ApplicationYieldVM {
   Date: Date
   DateText: string
   Accounts: ApplicationYieldAccountVM[]
}

export class ApplicationYieldAccountVM {
   AccountID: number
   AccountText: string
   Gain: number
   Percentual: number
}

export class PatrimonyVM {
   PatrimonyResume: PatrimonyResumeItem[];
   PatrimonyDistribution: PatrimonyDistributionItem[]
}

export enum PatrimonyResumeEnum { PreviousMonthBalance = 0, CurrentMonthIncome = 1, CurrentMonthExpense = 2, CurrentMonthBalance = 3 };
export class PatrimonyResumeItem {
   Type: PatrimonyResumeEnum;
   Value: number;
   Text: string;
   get Icon(): string {
      if (this.Type == PatrimonyResumeEnum.CurrentMonthIncome)
         return 'add_circle'
      else if (this.Type == PatrimonyResumeEnum.CurrentMonthExpense)
         return 'remove_circle'
      else if (this.Type == PatrimonyResumeEnum.CurrentMonthBalance)
         return 'monetization_on'
      else
         return 'lens'
   }
}

export class PatrimonyDistributionItem {
   Text: string;
   Value: number;
}
