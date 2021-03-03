import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusyService, IMonthSelectorService, Month } from '@elesse/shared';
import { Observable } from 'rxjs';
import { BalanceCache } from '../cache/cache.service';
import { BalanceEntity } from '../models/balance.model';

@Injectable({
   providedIn: 'root'
})
export class BalanceData {

   constructor(
      private Cache: BalanceCache,
      private busy: BusyService,
      private monthSelector: IMonthSelectorService,
      private http: HttpClient) {
      this.monthSelector.OnChange.subscribe(month => this.OnMonthChange(month));
      this.OnMonthChange(this.CurrentMonth);
   }

   private get CurrentMonth(): Month { return this.monthSelector.CurrentMonth; }
   public get ObserveBalances(): Observable<BalanceEntity[]> { return this.Cache.Observe; }

   private OnMonthChange(value: Month) {
      this.Cache.InitializeValue(value.ToCode());
      this.RefreshBalances();
   }

   public async RefreshBalances(): Promise<void> {
      try {
         this.busy.show();

         const url = `api/balances/list/${this.CurrentMonth.Year}/${this.CurrentMonth.Month}`;
         const values = await this.http.get<BalanceEntity[]>(url).toPromise();
         if (!values)
            return;

         this.Cache.SetBalances(this.CurrentMonth.ToCode(), values);
      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
