import { EnumVM } from 'src/app/shared/common/common.models';

export enum enCategoryType { None = 0, Expense = 1, Income = 2 };

export class Category {
   CategoryID: number;
   Text: string;
   Type: enCategoryType;
   ParentID: number;
   ParentRow?: Category;
   HierarchyText: string;
   SplitedText: string[];
}

export class CategoryType extends EnumVM<enCategoryType> {
   Categories: Category[] = [];
}
