import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { EntryEntity } from '../model/entries.model';
import { EntriesCache } from './cache.service';

describe('EntriesCache', () => {
   let service: EntriesCache;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(EntriesCache);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('SetEntries parses and raises observable with list', (done) => {
      const entries: EntryEntity[] = [
         EntryEntity.Parse({ AccountID: "account", Value: 1000, Paid: true, Sorting: 10 }),
         EntryEntity.Parse({ AccountID: "account", Value: -100, Paid: true, Sorting: 30 }),
         EntryEntity.Parse({ AccountID: "account", Value: -200.50, Paid: true, Sorting: 50 }),
         EntryEntity.Parse({ AccountID: "account", Value: 0.50, Paid: true, Sorting: 40 }),
         EntryEntity.Parse({ AccountID: "account", Value: -888, Sorting: 20 })
      ];
      const month = "201001";

      service.InitializeValue(month);
      service.GetObservable(month).subscribe(resultEntries => {
         if (resultEntries != null) {
            expect(resultEntries.length).toBe(5)
            done();
         }
      });

      service.SetEntries(month, entries)
   });

});
