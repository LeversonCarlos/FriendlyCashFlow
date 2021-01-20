import { EnumData } from "@elesse/shared";

export enum enCategoryType { None = 0, Expense = 1, Income = 2 }

export class CategoryType extends EnumData<enCategoryType> { }

export class CategoryEntity {
   CategoryID: string;
   Text: string;
   Type: enCategoryType;
   ParentID: string;
   HierarchyText: string;
}
