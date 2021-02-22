import { IMonthSelectorService } from "./month-selector.interface";
import { MonthSelectorService } from "./month-selector.service";

export const MonthSelectorProvider = {
   provide: IMonthSelectorService,
   useExisting: MonthSelectorService
};
