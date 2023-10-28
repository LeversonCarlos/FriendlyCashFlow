import { enCategoryType } from "../categories/categories.viewmodels"

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

export class MonthlyTargetHeaderVM {
   Date: Date;
   DateText: string;
   BalanceValue: number;
   TargetValue: number;
}

export class MonthlyTargetItemVM {
   Date: Date;
   DateText: string;
   Type: enCategoryType;
   SerieText: string;
   Value: number;
}

export class MonthlyTargetVM {
   Headers: MonthlyTargetHeaderVM[];
   Items: MonthlyTargetItemVM[];
}

export class MonthlyBudgetVM {
   PatternID: number
   Text: string
   CategoryID: number
   OverflowValue: number
   OverflowPercent: number
}

export class YearlyBudgetVM {
   CategoryID: number;
   CategoryText: string;

   BudgetValue: number | null;

   MonthValue: number | null;
   MonthPercentage: number;

   YearValue: number | null;
   YearPercentage: number;
}

export class ApplicationYieldVM {
   Date: Date
   DateText: string
   Accounts: ApplicationYieldAccountVM[]
}

export class ApplicationYieldAccountVM {
   AccountID: number
   AccountText: string
   OriginalGain: number;
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
