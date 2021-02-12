import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';

import { MonthSelectorService } from './month-selector.service';

describe('MonthSelectorService', () => {
   let service: MonthSelectorService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(MonthSelectorService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

});
