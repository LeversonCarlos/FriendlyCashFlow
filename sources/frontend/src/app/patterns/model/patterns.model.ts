import { enCategoryType } from "@elesse/categories";

export class PatternEntity {
   PatternID: string;
   Type: enCategoryType;
   CategoryID: string;
   Text: string;
   RowsCount: number;

   static Parse(value: any): PatternEntity {
      return Object.assign(new PatternEntity, {
         PatternID: value?.PatternID ?? null,
         Type: value?.Type ?? null,
         CategoryID: value?.CategoryID ?? null,
         Text: value?.Text ?? null,
         RowsCount: value?.RowsCount ?? 0
      });
   }

}
