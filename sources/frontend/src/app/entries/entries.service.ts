import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { BusyService } from '../shared/busy/busy.service';
import { EntriesCacheService } from './cache/entries-cache.service';
import { EntryEntity } from './entries.data';
import { MonthService } from './month/month.service';

@Injectable({
   providedIn: 'root'
})
export class EntriesService {

   constructor(private busy: BusyService,
      private http: HttpClient) {
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

   public async RefreshCache(): Promise<void> {
      try {
         this.busy.show();

         const url = `api/entries/list/${this._SelectedMonth.Year}/${this._SelectedMonth.Month}`;
         const values = await this.http.get<EntryEntity[]>(url).toPromise();
         if (!values)
            return;

         this.Cache.SetEntries(this.SelectedMonth.ToCode(), values);
      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
