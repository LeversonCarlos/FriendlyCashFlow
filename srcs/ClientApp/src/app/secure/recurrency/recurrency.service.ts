import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { EnumVM } from 'src/app/shared/common/common.models';
import { enRecurrencyType } from './recurrency.viewmodels';

@Injectable({
   providedIn: 'root'
})
export class RecurrencyService {

   constructor(private busy: BusyService, private http: HttpClient) { }

   // RECURRENCY TYPES
   public async getRecurrencyTypes(): Promise<EnumVM<enRecurrencyType>[]> {
      try {
         this.busy.show();
         const dataList = await this.http.get<EnumVM<enRecurrencyType>[]>("api/recurrencies/types")
            .pipe(map(items => items.map(item => Object.assign(new EnumVM<enRecurrencyType>(), item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

}
