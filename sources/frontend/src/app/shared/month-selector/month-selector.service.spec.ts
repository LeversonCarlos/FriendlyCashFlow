import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { Month } from './month';

import { MonthSelectorService } from './month-selector.service';

describe('MonthSelectorService', () => {
   // let service: MonthSelectorService;

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
      // service = TestBed.inject(MonthSelectorService);
   });

   it('should be created', () => {
      const service = new MonthSelectorService();
      expect(service).toBeTruthy();
   });

   it('changed current date must reflect the date passed by', () => {
      const service = new MonthSelectorService();
      service.CurrentMonth = new Month(new Date("2021-01-12"));
      expect(service.PreviousMonth.ToCode()).toEqual("202012");
      expect(service.NextMonth.ToCode()).toEqual("202102");
   });

   it('changed current date must emit OnChange', () => {
      const service = new MonthSelectorService();
      const month = new Month(new Date("2021-01-12"));
      const spy = spyOn(service.OnChange, 'emit');
      service.CurrentMonth = month;
      expect(spy).toHaveBeenCalledWith(month);
   });

});
