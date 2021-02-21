import { HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { TransfersData } from './transfers.data';
import { getSnapshot } from './transfers.data.spec';

describe('TransfersData', () => {
   let service: TransfersData;
   let httpMock: HttpTestingController;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(TransfersData);
      httpMock = TestBed.inject(HttpTestingController);
   });

   it('LoadTransfer with null snapshot parameter must result null', async () => {
      const result = await service.LoadTransfer(null);
      expect(result).toBeNull();
   });

   it('LoadTransfer with null snapshot.routeConfig parameter must result null', async () => {
      const snapshot = getSnapshot();
      const result = await service.LoadTransfer(snapshot);
      expect(result).toBeNull();
   });

   it('LoadTransfer with null snapshot.routeConfig.path parameter must result null', async () => {
      const snapshot = getSnapshot('');
      const result = await service.LoadTransfer(snapshot);
      expect(result).toBeNull();
   });

   it('LoadTransfer with "new" snapshot.routeConfig.path parameter must result new instance', async () => {
      const snapshot = getSnapshot('new');
      const result = await service.LoadTransfer(snapshot);
      expect(result).toBeTruthy();
      expect(result.TransferID).toBeNull();
   });

   it('LoadTransfer with "edit" snapshot.routeConfig.path but null transfer parameter must result null', async () => {
      const snapshot = getSnapshot('edit');
      const result = await service.LoadTransfer(snapshot);
      expect(result).toBeNull();
   });

   it('LoadTransfer with "edit" snapshot.routeConfig.path parameter and error from httpClient must result null', (done) => {
      const snapshot = getSnapshot('edit', { 'transfer': 'my-transfer-id' });

      service.LoadTransfer(snapshot).then(result => {
         expect(result).toBeNull();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/transfers/load"));
      httpRequest.error(new ErrorEvent('any http error'))
   });

   it('LoadTransfer with "edit" snapshot.routeConfig.path parameter and null return from httpClient must result null', (done) => {
      const snapshot = getSnapshot('edit', { 'transfer': 'my-transfer-id' });

      service.LoadTransfer(snapshot).then(result => {
         expect(result).toBeNull();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/transfers/load"));
      httpRequest.flush(null)
   });

   it('LoadTransfer with "edit" snapshot.routeConfig.path parameter and valid return from httpClient must result valid instance', (done) => {
      const snapshot = getSnapshot('edit', { 'transfer': 'my-transfer-id' });

      service.LoadTransfer(snapshot).then(result => {
         expect(result).toBeTruthy();
         expect(result.TransferID).toEqual(expected.TransferID);
         expect(result.ExpenseAccountID).toEqual(expected.ExpenseAccountID);
         expect(result.IncomeAccountID).toEqual(expected.IncomeAccountID);
         expect(result.Date).toEqual(new Date(expected.Date));
         expect(result.Value).toEqual(expected.Value);
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/transfers/load"));
      const expected = {
         TransferID: 'TransferID',
         ExpenseAccountID: 'ExpenseAccountID',
         IncomeAccountID: 'IncomeAccountID',
         Date: '2021-02-16',
         Value: 12.34
      };
      httpRequest.flush(expected)
   });

});
