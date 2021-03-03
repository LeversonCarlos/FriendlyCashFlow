import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { BalanceCache } from './cache.service';

describe('BalanceCache', () => {
   let service: BalanceCache;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(BalanceCache);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
