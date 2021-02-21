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

   it('SaveEntry with null parameter must result false', async () => {
      const result = await service.SaveEntry(null);
      expect(result).toBeFalse();
   });

   it('SaveEntry with valid parameter and error from httpClient must result false', (done) => {
      const param = EntryEntity.Parse({ EntryID: null });

      service.SaveEntry(param).then(result => {
         expect(result).toBeFalse();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/entries/insert"));
      httpRequest.error(new ErrorEvent('any http error'))
   });

   it('SaveEntry with valid parameter and valid return from httpClient must call refresh functions and result true', (done) => {
      const param = EntryEntity.Parse({ EntryID: 'my-entry-id' });
      const refreshEntriesSpy = spyOn(service, 'RefreshEntries').and.callFake(() => Promise.resolve());
      const refreshPatternsSpy = spyOn(TestBed.inject(PatternsData), 'RefreshPatterns').and.callFake(() => Promise.resolve());

      service.SaveEntry(param).then(result => {
         expect(result).toBeTrue();
         expect(refreshEntriesSpy).toHaveBeenCalled();
         expect(refreshPatternsSpy).toHaveBeenCalled();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/entries/update"));
      httpRequest.flush(null);
   });

});
