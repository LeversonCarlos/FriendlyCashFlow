import { HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, Params, Route } from '@angular/router';
import { TestsModule } from '@elesse/tests';
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

   it('should be created', () => {
      expect(service).toBeTruthy();
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
         expect(result.EntryValue).toEqual(expected.EntryValue);
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/entries/load"));
      const expected = {
         EntryID: 'EntryID',
         AccountID: 'AccountID',
         DueDate: '2021-02-16',
         EntryValue: 12.34
      };
      httpRequest.flush(expected)
   });

});

function getSnapshot(path: string = null, params: Params = null): ActivatedRouteSnapshot {
   let routeConfig: Route = null;
   if (path != null)
      routeConfig = { path: path }
   const snapshot: ActivatedRouteSnapshot = { url: null, routeConfig: routeConfig, params: params, queryParams: null, fragment: null, data: null, outlet: null, component: null, root: null, parent: null, firstChild: null, children: null, pathFromRoot: null, paramMap: null, queryParamMap: null };
   return snapshot;
}
