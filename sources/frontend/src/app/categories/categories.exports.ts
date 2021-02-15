export { CategoriesModule } from './categories.module';

export { CategoryEntity, enCategoryType, CategoryType } from './model/categories.model';
export { CategoriesData } from './data/categories.data';

import { CategoriesCache } from './cache/cache.service';
import { CategoriesData } from './data/categories.data';
export const CategoriesProviders = [CategoriesData, CategoriesCache];
