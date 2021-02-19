import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { enCategoryType } from '@elesse/categories';
import { BusyService } from '../../shared/busy/busy.service';
import { EntriesCache } from '../cache/cache.service';
import { EntryEntity } from '../model/entries.model';
import { Month, MonthSelectorService } from '@elesse/shared';
import { PatternsData } from '@elesse/patterns';
import { ActivatedRouteSnapshot } from '@angular/router';

@Injectable({
   providedIn: 'root'
})
export class EntriesData {

   constructor(private Cache: EntriesCache,
      private busy: BusyService, private monthSelector: MonthSelectorService,
      private patternsData: PatternsData,
      private http: HttpClient) {
      this.monthSelector.OnChange.subscribe(month => this.OnMonthChange(month));
      this.OnMonthChange(this.CurrentMonth);
   }

   private get CurrentMonth(): Month { return this.monthSelector.CurrentMonth; }
   public get ObserveEntries(): Observable<EntryEntity[]> { return this.Cache.Observe; }

   private OnMonthChange(value: Month) {
      this.Cache.InitializeValue(value.ToCode());
      this.RefreshEntries();
   }

   public async RefreshEntries(): Promise<void> {
      try {
         this.busy.show();

         const url = `api/entries/list/${this.CurrentMonth.Year}/${this.CurrentMonth.Month}`;
         const values = await this.http.get<EntryEntity[]>(url).toPromise();
         if (!values)
            return;

         this.Cache.SetEntries(this.CurrentMonth.ToCode(), values);
      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async LoadEntry(snapshot: ActivatedRouteSnapshot): Promise<EntryEntity> {
      try {
         this.busy.show();

         if (!snapshot || !snapshot.routeConfig || !snapshot.routeConfig.path)
            return null;

         if (snapshot.routeConfig.path == "new/income")
            return EntryEntity.Parse({ Pattern: { Type: enCategoryType.Income }, DueDate: this.monthSelector.DefaultDate });
         else if (snapshot.routeConfig.path == "new/expense")
            return EntryEntity.Parse({ Pattern: { Type: enCategoryType.Expense }, DueDate: this.monthSelector.DefaultDate });

         const entryID = snapshot.params?.entry;
         if (!entryID)
            return null;

         let entry = await this.http.get<EntryEntity>(`api/entries/load/${entryID}`).toPromise();
         if (!entry)
            return null;

         entry = EntryEntity.Parse(entry);
         return entry;

      }
      catch { return null; /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async SaveEntry(entry: EntryEntity): Promise<boolean> {
      try {
         this.busy.show();

         if (!entry)
            return false;

         if (entry.EntryID == null)
            await this.http.post("api/entries/insert", entry).toPromise();
         else
            await this.http.put("api/entries/update", entry).toPromise();

         await this.RefreshEntries();
         await this.patternsData.RefreshPatterns();
         return true;

      }
      catch { return false; /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
