import { Component, OnInit, Output, Input, EventEmitter, OnDestroy } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
   selector: 'fs-month-picker',
   templateUrl: './month-picker.component.html',
   styleUrls: ['./month-picker.component.scss']
})
export class MonthPickerComponent implements OnInit, OnDestroy {

   constructor() { }

   /* INIT */
   public ngOnInit() {
      const delay = 500;
      this.CurrentMonthChangeSubscription = this.CurrentMonthChangeDebouncer
         .pipe(
            debounceTime(delay),
            distinctUntilChanged()
         )
         .subscribe(value => this.CurrentMonthChanged.emit(value));
   }

   /* CURRENT MONTH */
   private currentMonth: Date
   public get CurrentMonth(): Date {
      return this.currentMonth;
   }
   @Input() public set CurrentMonth(val: Date) {
      if (!val) { return }
      this.PreviousMonth = new Date(new Date(val).setMonth(val.getMonth() - 1));
      this.currentMonth = val;
      this.NextMonth = new Date(new Date(val).setMonth(val.getMonth() + 1));
   }
   @Output() public CurrentMonthChanged: EventEmitter<Date> = new EventEmitter<Date>();
   private CurrentMonthChangeDebouncer: Subject<Date> = new Subject<Date>();
   private CurrentMonthChangeSubscription: Subscription | null;

   /* PREVIOUS MONTH */
   public PreviousMonth: Date;
   public OnPreviousMonthClick() { this.CurrentMonth = this.PreviousMonth; this.CurrentMonthChangeDebouncer.next(this.CurrentMonth); }

   /* NEXT MONTH */
   public NextMonth: Date;
   public OnNextMonthClick() { this.CurrentMonth = this.NextMonth; this.CurrentMonthChangeDebouncer.next(this.CurrentMonth); }

   public ngOnDestroy(): void {
      this.CurrentMonthChangeSubscription?.unsubscribe();
      this.CurrentMonthChangeSubscription = null;
   }

}
