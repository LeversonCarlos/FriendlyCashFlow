import { Injectable } from '@angular/core';

@Injectable({
   providedIn: 'root'
})
export class MonthService {

   constructor(private _Date: Date) {
      this._PreviousMonthDate = new Date(this._Date);
      this._PreviousMonthDate.setMonth(this._PreviousMonthDate.getMonth() - 1);
      this._NextMonthDate = new Date(this._Date);
      this._NextMonthDate.setMonth(this._NextMonthDate.getMonth() + 1);
   }

   public static Now(): MonthService {
      return new MonthService(new Date());
   }

   private _PreviousMonthDate: Date;
   public GetPreviousMonth(): MonthService {
      return new MonthService(this._PreviousMonthDate);
   }

   private _NextMonthDate: Date;
   public GetNextMonth(): MonthService {
      return new MonthService(this._NextMonthDate);
   }

   public ToCode(): string {
      const iSOString = this._Date.toISOString();
      return `${iSOString.substring(0, 4)}${iSOString.substring(5, 7)}`;
   }

   public get Year(): number { return this._Date.getFullYear(); }
   public get Month(): number { return this._Date.getMonth() + 1; }

}

export class MonthData {
   public Year: number;
   public Month: number;

}
