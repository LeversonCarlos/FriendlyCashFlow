import { TestBed } from '@angular/core/testing';
import { TestsModule } from '@elesse/tests';
import { Month } from './month';

describe('Month', () => {

   beforeEach(() => {
      TestBed.configureTestingModule({
         imports: [TestsModule]
      });
   });

   it('should be created', () => {
      const service = Month.GetToday();
      expect(service).toBeTruthy();
   });

   it('year and month properties must reflect current date', () => {
      const now = new Month(new Date("2021-12-10"));

      expect(now.Year).toEqual(2021);
      expect(now.Month).toEqual(12);
   });

   it('ToCode must reflect current date', () => {
      const now = new Month(new Date("2021-02-12"));

      expect(now.ToCode()).toEqual("202102");
   });

});
