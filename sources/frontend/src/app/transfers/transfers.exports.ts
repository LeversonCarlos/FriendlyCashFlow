export { TransfersModule } from './transfers.module';

export { TransferEntity } from './model/transfers.model';
export { TransfersData } from './data/transfers.data';

/*
import { EntriesCache } from './cache/cache.service';
*/

import { TransfersData } from './data/transfers.data';
export const TransfersProviders = [TransfersData /*,EntriesCache*/];
