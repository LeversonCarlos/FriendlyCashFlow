import { HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { TransferEntity } from '../model/transfers.model';
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

   it('RemoveTransfer with null parameter must result undefined', async () => {
      const result = await service.RemoveTransfer(null);
      expect(result).toBeUndefined();
   });

   it('RemoveTransfer with valid parameter and error from httpClient must result undefined', (done) => {
      const param = TransferEntity.Parse({ TransferID: 'my-transfer-id' });

      service.RemoveTransfer(param).then(result => {
         expect(result).toBeUndefined();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/transfers/delete"));
      httpRequest.error(new ErrorEvent('any http error'))
   });

   it('RemoveTransfer with valid parameter and valid return from httpClient must call refresh functions', (done) => {
      const param = TransferEntity.Parse({ TransferID: 'my-transfer-id' });
      const refreshTransfersSpy = spyOn(service, 'RefreshTransfers').and.callFake(() => Promise.resolve());

      service.RemoveTransfer(param).then(result => {
         expect(result).toBeUndefined();
         expect(refreshTransfersSpy).toHaveBeenCalled();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/transfers/delete"));
      httpRequest.flush(null);
   });

});
