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

});
