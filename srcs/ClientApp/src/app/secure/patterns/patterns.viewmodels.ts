import { enCategoryType, Category } from '../categories/categories.service';

export class Pattern {
   PatternID: number;
   Type: enCategoryType;
   CategoryID: number;
   CategoryRow: Category;
   Text: string;
   Count: number
}
