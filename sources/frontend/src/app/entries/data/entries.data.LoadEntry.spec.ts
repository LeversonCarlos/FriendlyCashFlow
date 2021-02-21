import { HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { EntriesData } from './entries.data';
import { getSnapshot } from './entries.data.spec';

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

   it('LoadEntry with null snapshot parameter must result null', async () => {
      const result = await service.LoadEntry(null);
      expect(result).toBeNull();
   });

   it('LoadEntry with null snapshot.routeConfig parameter must result null', async () => {
      const snapshot = getSnapshot();
      const result = await service.LoadEntry(snapshot);
      expect(result).toBeNull();
   });

   it('LoadEntry with null snapshot.routeConfig.path parameter must result null', async () => {
      const snapshot = getSnapshot('');
      const result = await service.LoadEntry(snapshot);
      expect(result).toBeNull();
   });

   it('LoadEntry with "new/income" snapshot.routeConfig.path parameter must result new instance', async () => {
      const snapshot = getSnapshot('new/income');
      const result = await service.LoadEntry(snapshot);
      expect(result).toBeTruthy();
      expect(result.EntryID).toBeNull();
   });

   it('LoadEntry with "new/expense" snapshot.routeConfig.path parameter must result new instance', async () => {
      const snapshot = getSnapshot('new/expense');
      const result = await service.LoadEntry(snapshot);
      expect(result).toBeTruthy();
      expect(result.EntryID).toBeNull();
   });

   it('LoadEntry with "edit" snapshot.routeConfig.path but null entry parameter must result null', async () => {
      const snapshot = getSnapshot('edit');
      const result = await service.LoadEntry(snapshot);
      expect(result).toBeNull();
   });

   it('LoadEntry with "edit" snapshot.routeConfig.path parameter and error from httpClient must result null', (done) => {
      const snapshot = getSnapshot('edit', { 'entry': 'my-entry-id' });

      service.LoadEntry(snapshot).then(result => {
         expect(result).toBeNull();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/entries/load"));
      httpRequest.error(new ErrorEvent('any http error'))
   });

   it('LoadEntry with "edit" snapshot.routeConfig.path parameter and null return from httpClient must result null', (done) => {
      const snapshot = getSnapshot('edit', { 'entry': 'my-entry-id' });

      service.LoadEntry(snapshot).then(result => {
         expect(result).toBeNull();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/entries/load"));
      httpRequest.flush(null)
   });

   it('LoadEntry with "edit" snapshot.routeConfig.path parameter and valid return from httpClient must result valid instance', (done) => {
      const snapshot = getSnapshot('edit', { 'entry': 'my-entry-id' });

      service.LoadEntry(snapshot).then(result => {
         expect(result).toBeTruthy();
         expect(result.EntryID).toEqual(expected.EntryID);
         expect(result.AccountID).toEqual(expected.AccountID);
         expect(result.DueDate).toEqual(new Date(expected.DueDate));
         expect(result.Value).toEqual(expected.Value);
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/entries/load"));
      const expected = {
         EntryID: 'EntryID',
         AccountID: 'AccountID',
         DueDate: '2021-02-16',
         Value: 12.34
      };
      httpRequest.flush(expected)
   });

});
