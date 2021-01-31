import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { enCategoryType } from '@elesse/categories';
import { BusyService } from '../shared/busy/busy.service';
import { EntriesCacheService } from './cache/entries-cache.service';
import { EntryEntity } from './model/entries.model';
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
      this.Cache.InitializeValue(value.ToCode());
   }

   private Cache: EntriesCacheService;
   public ObserveEntries = (): Observable<EntryEntity[]> =>
      this.Cache.GetObservable(this.SelectedMonth.ToCode());

   public async RefreshCache(): Promise<void> {
      try {
         this.busy.show();

         const url = `api/entries/list/${this.SelectedMonth.Year}/${this.SelectedMonth.Month}`;
         const values = await this.http.get<EntryEntity[]>(url).toPromise();
         if (!values)
            return;

         this.Cache.SetEntries(this.SelectedMonth.ToCode(), values);
      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async LoadEntry(entryID: string): Promise<EntryEntity> {
      try {
         this.busy.show();

         if (!entryID)
            return null;

         if (entryID == 'new-income')
            return Object.assign(new EntryEntity, { Pattern: { Type: enCategoryType.Income } });
         else if (entryID == 'new-expense')
            return Object.assign(new EntryEntity, { Pattern: { Type: enCategoryType.Expense } });

         let value = await this.http.get<EntryEntity>(`api/entries/load/${entryID}`).toPromise();
         if (!value)
            return null;

         value = Object.assign(new EntryEntity, value);
         return value;

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async SaveEntry(entry: EntryEntity): Promise<boolean> {
      try {
         this.busy.show();

         if (!entry)
            return false;

         console.log(entry);
         /*
         if (entry.EntryID == null)
            await this.http.post("api/entries/insert", entry).toPromise();
         else
            await this.http.put("api/entries/update", entry).toPromise();
         */

         return true;

      }
      catch { return false; /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
