import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { TransfersData } from './transfers.data';

describe('TransfersData', () => {
   let service: TransfersData;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(TransfersData);
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

});
