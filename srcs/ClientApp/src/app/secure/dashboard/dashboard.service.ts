import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { Balance } from './dashboard.viewmodels';
import { Entry } from '../entries/entries.viewmodels';

@Injectable({
   providedIn: 'root'
})
export class DashboardService {

   constructor(private busy: BusyService,
      private http: HttpClient) { }

   // BALANCES
   public async getBalances(excludeTransfers: boolean = false): Promise<Balance[]> {
      try {
         this.busy.show();
         const year = (new Date()).getFullYear();
         const month = (new Date()).getMonth() + 1;
         const url = `api/dashboard/balance/${year}/${month}/${(excludeTransfers ? 1 : 0)}`;
         const dataList = await this.http.get<Balance[]>(url)
            .pipe(map(items => items.map(item => Object.assign(new Balance, item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // BALANCES
   public async getEntries(): Promise<Entry[]> {
      try {
         this.busy.show();
         const url = `api/dashboard/entries`;
         const dataList = await this.http.get<Entry[]>(url)
            .pipe(map(items => items.map(item => Object.assign(new Entry, item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

}
