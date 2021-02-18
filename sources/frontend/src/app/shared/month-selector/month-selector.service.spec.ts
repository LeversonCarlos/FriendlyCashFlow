import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { Month } from './month';
import { MonthSelectorService } from './month-selector.service';

describe('MonthSelectorService', () => {

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
   });

   it('should be created', () => {
      const service = new MonthSelectorService();
      expect(service).toBeTruthy();
   });

   it('initial value for current month should be current date', () => {
      const now = Month.GetToday();
      const service = new MonthSelectorService();
      expect(service.CurrentMonth.ToCode()).toEqual(now.ToCode());
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

   it('changed to current month must define today as default date', () => {
      const service = new MonthSelectorService();
      const today = new Date();
      service.CurrentMonth = new Month(today);
      expect(service.DefaultDate.getFullYear()).toEqual(today.getFullYear());
      expect(service.DefaultDate.getMonth()).toEqual(today.getMonth());
      expect(service.DefaultDate.getDate()).toEqual(today.getDate());
   });

   it('changed to month different from current must define default date as first day on that month', () => {
      const service = new MonthSelectorService();
      const today = new Date("2010-07-25");
      service.CurrentMonth = new Month(today);
      expect(service.DefaultDate.getFullYear()).toEqual(2010);
      expect(service.DefaultDate.getMonth() + 1).toEqual(7);
      expect(service.DefaultDate.getDate()).toEqual(1);
   });

});
