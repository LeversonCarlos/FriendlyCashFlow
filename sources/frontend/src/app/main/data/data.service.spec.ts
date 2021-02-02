import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { MainData } from './data.service';

describe('MainData', () => {
   let service: MainData;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(MainData);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
