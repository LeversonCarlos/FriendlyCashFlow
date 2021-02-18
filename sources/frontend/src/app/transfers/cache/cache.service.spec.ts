import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { CacheService } from './cache.service';

describe('CacheService', () => {
   let service: CacheService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(CacheService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
