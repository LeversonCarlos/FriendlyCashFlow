import { EventEmitter, Injectable } from "@angular/core";
import { Month } from "./month";

export interface MonthSelectorServiceInterface {
   CurrentMonth: Month;
   readonly PreviousMonth: Month;
   readonly NextMonth: Month;
   readonly DefaultDate: Date;
   OnChange: EventEmitter<Month>
}

@Injectable({
   providedIn: 'root'
})
export abstract class IMonthSelectorService implements MonthSelectorServiceInterface {
   abstract CurrentMonth: Month;
   abstract get PreviousMonth(): Month;
   abstract get NextMonth(): Month;
   abstract get DefaultDate(): Date;
   abstract OnChange: EventEmitter<Month>;
}
