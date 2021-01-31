import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { EntriesData } from './entries.data';
import { MonthService } from '../month/month.service';

describe('EntriesData', () => {
   let service: EntriesData;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      service = TestBed.inject(EntriesData);
   });

   it('should be created', () => {
      expect(service).toBeTruthy();
   });

   it('initial value for selected month should be current date', () => {
      const now = MonthService.Now();
      expect(service.SelectedMonth.ToCode()).toEqual(now.ToCode());
   });

});
