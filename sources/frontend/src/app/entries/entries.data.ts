export class AccountEntries {
   AccountID: string;
   Balance: BalanceItemEntity;
   Days: DayEntries[] = []
}

export class DayEntries {
   Day: Date;
   Balance: BalanceItemEntity
   Entries: EntryEntity[] = []
}

export class EntryGroupEntity {
   Day: Date;
   Entries: EntryEntity[] = []
   Balance: BalanceEntity
}

export class EntryEntity {
   EntryID: string;
   Pattern: PatternEntity;

   AccountID: string;
   DueDate: Date;
   EntryValue: number;

   Paid: boolean;
   PayDate?: Date;

   Balance: BalanceEntity = new BalanceEntity();
   Sorting: number;
   SearchDate: Date

   static Parse(value: any): EntryEntity {
      return Object.assign(new EntryEntity, value);
   }

}

export class BalanceEntity {
   Account: BalanceItemEntity = new BalanceItemEntity();
   Total: BalanceItemEntity = new BalanceItemEntity();
}

export class BalanceItemEntity {
   ExpectedBalance: number = 0.00;
   RealizedBalance: number = 0.00;
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
