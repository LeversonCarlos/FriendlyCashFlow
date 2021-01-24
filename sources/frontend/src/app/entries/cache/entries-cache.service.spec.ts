import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { EntryEntity } from '../entries.data';
import { EntryEntityMocker } from '../entries.data.spec';
import { EntriesCacheService } from './entries-cache.service';

describe('EntriesCacheService', () => {
   let service: EntriesCacheService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(EntriesCacheService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('ObserveAccounts InitialValue MustResult Null', (done) => {
      const entries: EntryEntity[] = [
         EntryEntity.Parse({ AccountID: "account", EntryValue: 1000, Paid: true,Sorting: 10 }),
         EntryEntity.Parse({ AccountID: "account", EntryValue: -100, Paid: true, Sorting: 30 }),
         EntryEntity.Parse({ AccountID: "account", EntryValue: -200.50, Paid: true, Sorting: 50 }),
         EntryEntity.Parse({ AccountID: "account", EntryValue: 0.50, Paid: true, Sorting: 40 }),
         EntryEntity.Parse({ AccountID: "account", EntryValue: -888, Sorting: 20 })
      ];
      const month = "202101";

      service.InitializeValue(month);
      service.GetObservable(month).subscribe(resultEntries => {
         expect(resultEntries).not.toBeNull();
         expect(resultEntries.length).toBe(5)
         expect(resultEntries[4].Balance.Total.ExpectedBalance).toBe(-188)
         expect(resultEntries[4].Balance.Total.RealizedBalance).toBe(700)
         expect(resultEntries[4].EntryValue).toBe(-200.50)
         done();
      });

      service.SetEntries(month, entries)

   });

});
