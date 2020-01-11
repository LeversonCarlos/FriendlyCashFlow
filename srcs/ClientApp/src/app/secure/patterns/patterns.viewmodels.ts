import { enCategoryType } from '../categories/categories.service';

export class Pattern {
   PatternID: number;
   Type: enCategoryType;
   CategoryID: number;
   Text: string;
   Count: number
}
