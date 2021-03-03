import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { BalanceData } from './data.service';

describe('BalanceData', () => {
   let service: BalanceData;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(BalanceData);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
