import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { MonthService } from './month.service';

describe('MonthService', () => {
   // let service: MonthService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      // service = TestBed.inject(MonthService);
   });

   it('should be created', () => {
      const service = MonthService.Now();
      expect(service).toBeTruthy();
   });

   it('next month should instantiate a month in the future', () => {
      const now = new MonthService(new Date("2021-12-10"));

      const next = now.GetNextMonth();

      expect(next.ToCode()).toEqual("202201")
   });

   it('previous month should instantiate a month in the past', () => {
      const now = new MonthService(new Date("2022-01-10"));

      const previous = now.GetPreviousMonth();

      expect(previous.ToCode()).toEqual("202112")
   });

});
