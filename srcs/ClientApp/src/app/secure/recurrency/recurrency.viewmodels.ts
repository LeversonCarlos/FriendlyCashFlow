export enum enRecurrencyType { Fixed = 0, Weekly = 1, Monthly = 2, Bimonthly = 3, Quarterly = 4, SemiYearly = 5, Yearly = 6 }

export class Recurrency {
   Type: enRecurrencyType = enRecurrencyType.Fixed;
   Count: number;
}
