import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { PatternsCacheService } from './patterns-cache.service';

describe('PatternsCacheService', () => {
   let service: PatternsCacheService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(PatternsCacheService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
