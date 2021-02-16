export { PatternsModule } from './patterns.module';

export { PatternEntity } from './model/patterns.model';
export { PatternsData } from './data/data.service';

import { PatternsData } from './data/data.service';
import { PatternsCache } from './cache/cache.service';
export const PatternsProviders = [PatternsData, PatternsCache];
