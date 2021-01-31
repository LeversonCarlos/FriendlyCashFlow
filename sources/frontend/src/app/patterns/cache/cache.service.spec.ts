import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { PatternsCache } from './cache.service';

describe('PatternsCache', () => {
   let service: PatternsCache;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(PatternsCache);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
