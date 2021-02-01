import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { CategoriesCache } from './cache.service';

describe('CategoriesCache', () => {
   let service: CategoriesCache;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(CategoriesCache);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
