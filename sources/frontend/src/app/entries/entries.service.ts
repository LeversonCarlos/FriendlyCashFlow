import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { EntriesCacheService } from './cache/entries-cache.service';
import { EntryEntity } from './entries.data';
import { MonthService } from './month/month.service';

@Injectable({
   providedIn: 'root'
})
export class EntriesService {

   constructor() {
      this.Cache = new EntriesCacheService();
      this.SelectedMonth = MonthService.Now();
   }

   private _SelectedMonth: MonthService;
   public get SelectedMonth(): MonthService { return this._SelectedMonth; }
   public set SelectedMonth(value: MonthService) {
      this._SelectedMonth = value;
      this.Cache.InitializeValue(this._SelectedMonth.ToCode());
   }

   private Cache: EntriesCacheService;
   public ObserveEntries = (): Observable<EntryEntity[]> =>
      this.Cache.GetObservable(this.SelectedMonth.ToCode());

}
