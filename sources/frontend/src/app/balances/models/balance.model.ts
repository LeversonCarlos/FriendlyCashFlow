export class BalanceEntity {
   BalanceID: string;

   AccountID: string;
   Date: Date;

   ExpectedValue: number;
   RealizedValue: number;

   Sorting: number;

   static Parse(value: any): BalanceEntity {
      return Object.assign(new BalanceEntity, {
         BalanceID: value.BalanceID ?? null,
         AccountID: value.AccountID ?? null,
         Date: value.Date ? new Date(value.Date) : null,
         ExpectedValue: value.ExpectedValue ?? null,
         RealizedValue: value.RealizedValue ?? null,
         Sorting: value.Sorting ?? 0
      });
   }

}
