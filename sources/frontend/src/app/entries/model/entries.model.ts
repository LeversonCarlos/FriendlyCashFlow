import { PatternEntity } from "@elesse/patterns";

export class EntryEntity {
   EntryID: string;
   Pattern: PatternEntity;

   AccountID: string;
   DueDate: Date;
   EntryValue: number;

   Paid: boolean;
   PayDate?: Date;

   Sorting: number;
   SearchDate: Date

   static Parse(value: any): EntryEntity {
      return Object.assign(new EntryEntity, value);
   }

}
