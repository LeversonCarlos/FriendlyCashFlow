import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { CategoriesData } from './categories.data';

describe('CategoriesData', () => {

   let service: CategoriesData;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(CategoriesData);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
