import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Transfer } from './transfers.viewmodels';

@Injectable({
   providedIn: 'root'
})
export class TransfersService {

   constructor(private busy: BusyService,
      private http: HttpClient, private router: Router) { }

   // GET DATA
   public async getData(transferID: string): Promise<Transfer> {
      try {
         this.busy.show();
         const data = await this.http.get<Transfer>(`api/transfers/${transferID}`)
            .pipe(map(item => Object.assign(new Transfer, item)))
            .toPromise();
         return data;
      }
      catch (ex) { console.error(ex); return null; }
      finally { this.busy.hide(); }
   }

   // SAVE DATA
   public async saveData(value: Transfer): Promise<boolean> {
      try {
         this.busy.show();
         let result: Transfer = null;
         if (!value.TransferID || value.TransferID == '') {
            result = await this.http.post<Transfer>(`api/transfers`, value).toPromise();
         }
         else {
            result = await this.http.put<Transfer>(`api/transfers/${value.TransferID}`, value).toPromise();
         }
         return result != null;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // REMOVE DATA
   public async removeData(value: Transfer): Promise<boolean> {
      try {
         this.busy.show();
         const result = await this.http.delete<boolean>(`api/transfers/${value.TransferID}`).toPromise();
         return result;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

}
