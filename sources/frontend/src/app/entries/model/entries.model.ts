import { PatternEntity } from "@elesse/patterns";

export class EntryEntity {
   EntryID: string;
   Pattern: PatternEntity;

   AccountID: string;
   DueDate: Date;
   EntryValue: number;

   Paid: boolean;
   PayDate?: Date;

   SearchDate: Date;
   Sorting: number;

   static Parse(value: any): EntryEntity {
      return Object.assign(new EntryEntity, {
         EntryID: value.EntryID ?? null,
         Pattern: PatternEntity.Parse(value.Pattern),
         AccountID: value.AccountID ?? null,
         DueDate: value.DueDate ? new Date(value.DueDate) : null,
         EntryValue: value.EntryValue ?? null,
         Paid: value.Paid ?? false,
         PayDate: value.PayDate ? new Date(value.PayDate) : null,
         SearchDate: value.SearchDate ? new Date(value.SearchDate) : null,
         Sorting: value.Sorting ?? 0
      });
   }

}
