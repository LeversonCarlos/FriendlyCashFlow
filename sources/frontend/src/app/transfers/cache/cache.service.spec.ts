import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { TransferEntity } from '../model/transfers.model';
import { TransfersCache } from './cache.service';

describe('TransfersCache', () => {
   let service: TransfersCache;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(TransfersCache);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('SetTransfers parses and raises observable with list', (done) => {
      const entries: TransferEntity[] = [
         TransferEntity.Parse({ ExpenseAccountID: "account1", IncomeAccountID: "account2", Value: 1000, Sorting: 10 }),
         TransferEntity.Parse({ ExpenseAccountID: "account1", IncomeAccountID: "account2", Value: -100, Sorting: 30 }),
         TransferEntity.Parse({ ExpenseAccountID: "account1", IncomeAccountID: "account2", Value: -200.50, Sorting: 50 }),
         TransferEntity.Parse({ ExpenseAccountID: "account2", IncomeAccountID: "account3", Value: 0.50, Sorting: 40 }),
         TransferEntity.Parse({ ExpenseAccountID: "account3", IncomeAccountID: "account1", Value: -888, Sorting: 70 })
      ];
      const month = "201001";

      service.InitializeValue(month);
      service.GetObservable(month).subscribe(resultTransfers => {
         if (resultTransfers != null) {
            expect(resultTransfers.length).toBe(5)
            done();
         }
      });

      service.SetTransfers(month, entries)
   });

});
