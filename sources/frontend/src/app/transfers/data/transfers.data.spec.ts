import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { TransfersDataService } from './transfers.data';

describe('TransfersDataService', () => {
   let service: TransfersDataService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(TransfersDataService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
