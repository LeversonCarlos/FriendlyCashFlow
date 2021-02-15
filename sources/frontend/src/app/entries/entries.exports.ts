export { EntriesModule } from './entries.module';

export { EntryEntity } from './model/entries.model';
export { EntriesData } from './data/entries.data';

import { EntriesCache } from './cache/cache.service';
import { EntriesData } from './data/entries.data';
export const EntriesProviders = [EntriesData, EntriesCache];
