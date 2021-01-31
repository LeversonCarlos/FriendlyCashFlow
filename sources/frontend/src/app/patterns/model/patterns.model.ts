import { enCategoryType } from "@elesse/categories";

export class PatternEntity {
   PatternID: string;
   Type: enCategoryType;
   CategoryID: string;
   Text: string;

   static Parse(value: any): PatternEntity {
      return Object.assign(new PatternEntity, value);
   }

}
