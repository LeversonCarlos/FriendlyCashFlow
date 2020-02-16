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
   Value: number
   AverageValue: number
   Percent: number
   Childs: CategoryGoalsVM[]
}

export class EntriesParetoVM {
   Text: string
   Value: number
   Pareto: number
}

export class MonthlyTargetVM {
   SearchDate: Date
   Text: string
   IncomeValue: number
   IncomeAverage: number
   IncomeTarget: number
   ExpenseValue: number
   ExpenseAverage: number
   ExpenseTarget: number
   Balance: number
}
