import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { Balance } from './dashboard.viewmodels';

@Injectable({
   providedIn: 'root'
})
export class DashboardService {

   constructor(private busy: BusyService,
      private http: HttpClient) { }

   // BALANCES
   public async getBalances(): Promise<Balance[]> {
      try {
         this.busy.show();
         const year = (new Date()).getFullYear();
         const month = (new Date()).getMonth() + 1;
         const url = `api/dashboard/balance/${year}/${month}`;
         const dataList = await this.http.get<Balance[]>(url)
            .pipe(map(items => items.map(item => Object.assign(new Balance, item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

}
