import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { PatternsData } from './data.service';

describe('PatternsData', () => {
   let service: PatternsData;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(PatternsData);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
