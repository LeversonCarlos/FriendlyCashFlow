import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { BalanceEntity } from '../models/balance.model';
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

   it('SetBalances must raises observable with parsed list', (done) => {
      const entities: BalanceEntity[] = [
         BalanceEntity.Parse({ AccountID: "account", ExpectedValue: 1000.00, Sorting: 10 }),
         BalanceEntity.Parse({ AccountID: "account", ExpectedValue: -100.00, Sorting: 30 }),
         BalanceEntity.Parse({ AccountID: "account", ExpectedValue: -200.50, Sorting: 50 }),
         BalanceEntity.Parse({ AccountID: "account", ExpectedValue: -888.00, Sorting: 20 })
      ];
      const month = "201001";

      service.InitializeValue(month);
      service.GetObservable(month).subscribe(result => {
         if (result != null) {
            expect(result.length).toBe(4)
            done();
         }
      });

      service.SetBalances(month, entities)
   });

});
