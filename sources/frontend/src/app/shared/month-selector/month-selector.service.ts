import { EventEmitter, Injectable, Output } from '@angular/core';
import { Month } from './month';

@Injectable({
   providedIn: 'root'
})
export class MonthSelectorService {

   constructor() {
      this.CurrentMonth = Month.GetToday();
   }

   @Output()
   public OnChange = new EventEmitter<Month>();

   private _CurrentMonth: Month;
   public get CurrentMonth(): Month { return this._CurrentMonth; }
   public set CurrentMonth(value: Month) {
      if (this._CurrentMonth == value)
         return;
      this._CurrentMonth = value;
      this.OnChange.emit(value);
   }

   public get PreviousMonth(): Month {
      let date = new Date(this.CurrentMonth.Date);
      date.setMonth(date.getMonth() - 1);
      return new Month(date);
   }

   public get NextMonth(): Month {
      let date = new Date(this.CurrentMonth.Date);
      date.setMonth(date.getMonth() + 1);
      return new Month(date);
   }

}
