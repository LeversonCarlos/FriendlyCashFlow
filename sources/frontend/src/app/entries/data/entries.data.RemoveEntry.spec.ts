import { HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { PatternsData } from '@elesse/patterns';
import { TestsModule } from '@elesse/tests';
import { EntryEntity } from '../model/entries.model';
import { EntriesData } from './entries.data';

describe('EntriesData', () => {
   let service: EntriesData;
   let httpMock: HttpTestingController;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(EntriesData);
      httpMock = TestBed.inject(HttpTestingController);
   });

   it('RemoveEntry with null parameter must result undefined', async () => {
      const result = await service.RemoveEntry(null);
      expect(result).toBeUndefined();
   });

   it('RemoveEntry with valid parameter and error from httpClient must result undefined', (done) => {
      const param = EntryEntity.Parse({ EntryID: 'my-entry-id' });

      service.RemoveEntry(param).then(result => {
         expect(result).toBeUndefined();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/entries/delete"));
      httpRequest.error(new ErrorEvent('any http error'))
   });

   it('RemoveEntry with valid parameter and valid return from httpClient must call refresh functions', (done) => {
      const param = EntryEntity.Parse({ EntryID: 'my-entry-id' });
      const refreshEntriesSpy = spyOn(service, 'RefreshEntries').and.callFake(() => Promise.resolve());
      const refreshPatternsSpy = spyOn(TestBed.inject(PatternsData), 'RefreshPatterns').and.callFake(() => Promise.resolve());

      service.RemoveEntry(param).then(result => {
         expect(result).toBeUndefined();
         expect(refreshEntriesSpy).toHaveBeenCalled();
         expect(refreshPatternsSpy).toHaveBeenCalled();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/entries/delete"));
      httpRequest.flush(null);
   });

});
