import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { TransfersCache } from './cache.service';

describe('TransfersCache', () => {
   let service: TransfersCache;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(TransfersCache);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
