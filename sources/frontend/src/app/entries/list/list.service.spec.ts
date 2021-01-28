import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { EntryEntity } from '../entries.data';
import { ConvertService } from './list.service';

describe('ConvertService', () => {
   let service: ConvertService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(ConvertService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('GetEntriesAccounts with null parameter shold result empty array', () => {
      const entries: EntryEntity[] = null
      expect(service.GetEntriesAccounts(entries)).toEqual([])
   });

   it('GetEntriesAccounts with empty parameter should result empty array', () => {
      const entries: EntryEntity[] = []
      expect(service.GetEntriesAccounts(entries)).toEqual([])
   });

});
