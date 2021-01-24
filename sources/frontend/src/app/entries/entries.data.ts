export class EntryEntity {
   EntryID: string;
   Pattern: PatternEntity;

   AccountID: string;
   DueDate: Date;
   EntryValue: number;

   Paid: boolean;
   PayDate?: Date;

   static Parse(value: any): EntryEntity {
      return Object.assign(new EntryEntity, value);
   }

}

export enum enPatternType { Expense = 1, Income = 2 }

export class PatternEntity {
   PatternID: string;
   Type: enPatternType;
   CategoryID: string;
   Text: string;

   static Parse(value: any): PatternEntity {
      return Object.assign(new PatternEntity, value);
   }

}
