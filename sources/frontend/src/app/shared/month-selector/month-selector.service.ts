import { EventEmitter, Injectable, Output } from '@angular/core';
import { Month } from './month';
import { IMonthSelectorService } from './month-selector.interface';

@Injectable({
   providedIn: 'root'
})
export class MonthSelectorService implements IMonthSelectorService {

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

      const today = new Date();
      if (value.Year == today.getFullYear() && value.Month == today.getMonth() + 1)
         this._DefaultDate = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 12);
      else
         this._DefaultDate = new Date(value.Year, (value.Month - 1), 1, 12);

      this.OnChange.emit(value);
   }

   private _DefaultDate: Date;
   public get DefaultDate(): Date { return this._DefaultDate; }

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
