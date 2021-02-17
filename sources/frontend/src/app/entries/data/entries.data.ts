import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { enCategoryType } from '@elesse/categories';
import { BusyService } from '../../shared/busy/busy.service';
import { EntriesCache } from '../cache/cache.service';
import { EntryEntity } from '../model/entries.model';
import { Month, MonthSelectorService } from '@elesse/shared';
import { PatternsData } from '@elesse/patterns';

@Injectable({
   providedIn: 'root'
})
export class EntriesData {

   constructor(private busy: BusyService,
      private Cache: EntriesCache,
      private patternsData: PatternsData,
      private monthSelector: MonthSelectorService,
      private http: HttpClient) {
      this.monthSelector.OnChange.subscribe(month => this.OnMonthChange(month));
      this.OnMonthChange(this.CurrentMonth);
   }

   private get CurrentMonth(): Month { return this.monthSelector.CurrentMonth; }
   public DefaultDueDate: Date;

   private OnMonthChange(value: Month) {
      const today = new Date();
      if (value.Year == today.getFullYear() && value.Month == today.getMonth() + 1)
         this.DefaultDueDate = new Date(today.getFullYear(), today.getMonth() + 1, today.getDate(), 12);
      else
         this.DefaultDueDate = new Date(value.Year, (value.Month - 1), 1, 12);
      this.Cache.InitializeValue(value.ToCode());
      this.RefreshEntries();
   }

   public get ObserveEntries(): Observable<EntryEntity[]> { return this.Cache.Observe; }

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

   public async LoadEntry(entryID: string): Promise<EntryEntity> {
      try {
         this.busy.show();

         if (!entryID)
            return null;

         if (entryID == 'new-income')
            return Object.assign(new EntryEntity, { Pattern: { Type: enCategoryType.Income, DueDate: this.DefaultDueDate } });
         else if (entryID == 'new-expense')
            return Object.assign(new EntryEntity, { Pattern: { Type: enCategoryType.Expense, DueDate: this.DefaultDueDate } });

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
