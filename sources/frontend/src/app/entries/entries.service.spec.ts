import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { EntriesService } from './entries.service';
import { MonthService } from './month/month.service';

describe('EntriesService', () => {
   let service: EntriesService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(EntriesService);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('initial value for selected month should be current date', () => {
      const now = MonthService.Now();
      expect(service.SelectedMonth.ToCode()).toEqual(now.ToCode());
   });

});
