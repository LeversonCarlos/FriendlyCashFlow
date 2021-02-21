import { HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { PatternsData } from '@elesse/patterns';
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

   it('SaveTransfer with null parameter must result false', async () => {
      const result = await service.SaveTransfer(null);
      expect(result).toBeFalse();
   });

   it('SaveTransfer with valid parameter and error from httpClient must result false', (done) => {
      const param = TransferEntity.Parse({ TransferID: null });

      service.SaveTransfer(param).then(result => {
         expect(result).toBeFalse();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/transfers/insert"));
      httpRequest.error(new ErrorEvent('any http error'))
   });

   it('SaveTransfer with valid parameter and valid return from httpClient must call refresh functions and result true', (done) => {
      const param = TransferEntity.Parse({ TransferID: 'my-transfer-id' });
      const refreshTransfersSpy = spyOn(service, 'RefreshTransfers').and.callFake(() => Promise.resolve());

      service.SaveTransfer(param).then(result => {
         expect(result).toBeTrue();
         expect(refreshTransfersSpy).toHaveBeenCalled();
         done();
      });

      const httpRequest = httpMock.expectOne(x => x.url.startsWith("api/transfers/update"));
      httpRequest.flush(null);
   });

});
