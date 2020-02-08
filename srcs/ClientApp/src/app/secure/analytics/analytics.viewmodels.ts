export class FilterData {
   public Month: Date

   public setData(month: Date) {
      this.Month = month
   }

}

export class CategoryGoalsVM {
   Text: string
   Value: number
   Percent: number
   Childs: CategoryGoalsVM[]
}
