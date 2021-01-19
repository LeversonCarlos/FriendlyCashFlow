import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { CategoriesService } from './categories.service';

describe('CategoriesService', () => {

   let service: CategoriesService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(CategoriesService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
