import { HttpTestingController } from '@angular/common/http/testing';
import { fakeAsync, TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { TransfersData } from './transfers.data';

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

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('LoadTransfer with null parameter must result null', async () => {
      const param: string = null;
      const result = await service.LoadTransfer(param);
      expect(result).toBeNull();
   });

   it('LoadTransfer with empty parameter must result null', async () => {
      const param: string = '';
      const result = await service.LoadTransfer(param);
      expect(result).toBeNull();
   });

   it('LoadTransfer with "new" parameter must result new instance', async () => {
      const param: string = 'new';
      const result = await service.LoadTransfer(param);
      expect(result).toBeTruthy();
      expect(result.TransferID).toBeNull();
   });

   it('LoadTransfer with null return from httpClient must result null', () => {
      const param: string = 'my-transfer-id';

      service.LoadTransfer(param).then(result => {
         expect(result).toBeNull();
      });

      const httpRequest = httpMock.expectOne(() => true);
      httpRequest.error(new ErrorEvent('any http error'))

      httpMock.verify()
   });

   it('LoadTransfer with valid return from httpClient must result valid instance', () => {
      const param: string = 'my-transfer-id';

      service.LoadTransfer(param).then(result => {
         expect(result).toBeTruthy();
         expect(result.TransferID).toEqual(expected.TransferID);
         expect(result.ExpenseAccountID).toEqual(expected.ExpenseAccountID);
         expect(result.IncomeAccountID).toEqual(expected.IncomeAccountID);
         expect(result.Date).toEqual(new Date(expected.Date));
         expect(result.Value).toEqual(expected.Value);
      });

      const httpRequest = httpMock.expectOne(() => true);
      const expected = {
         TransferID: 'TransferID',
         ExpenseAccountID: 'ExpenseAccountID',
         IncomeAccountID: 'IncomeAccountID',
         Date: '2021-02-16',
         Value: 12.34
      };
      httpRequest.flush(expected)

      httpMock.verify()
   });

});
