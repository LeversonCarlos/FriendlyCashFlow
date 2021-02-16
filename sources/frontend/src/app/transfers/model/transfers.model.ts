export class TransferEntity {
   TransferID: string;
   ExpenseAccountID: string;
   IncomeAccountID: string;
   Date: Date;
   Value: number;

   static Parse(value: any): TransferEntity {
      return Object.assign(new TransferEntity, {
         TransferID: value.TransferID ?? null,
         ExpenseAccountID: value.ExpenseAccountID ?? null,
         IncomeAccountID: value.IncomeAccountID ?? null,
         Date: value.Date ? new Date(value.Date) : null,
         Value: value.Value ?? null
      });
   }

}
