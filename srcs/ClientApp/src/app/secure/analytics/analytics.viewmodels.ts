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
