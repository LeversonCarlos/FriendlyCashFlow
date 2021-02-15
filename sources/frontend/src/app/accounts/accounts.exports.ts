export { AccountsModule } from './accounts.module';

export { AccountEntity, enAccountType, AccountType } from './model/accounts.model';
export { AccountsData } from './data/accounts.data';

import { AccountsCache } from './cache/cache.service';
import { AccountsData } from './data/accounts.data';
export const AccountsProviders = [AccountsData, AccountsCache];
