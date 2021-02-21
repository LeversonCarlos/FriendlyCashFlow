import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { BusyService, MessageService, Month, MonthSelectorService } from '@elesse/shared';
import { Observable } from 'rxjs';
import { TransfersCache } from '../cache/cache.service';
import { TransferEntity } from '../model/transfers.model';

@Injectable({
   providedIn: 'root'
})
export class TransfersData {

   constructor(private Cache: TransfersCache,
      private busy: BusyService, private message: MessageService, private monthSelector: MonthSelectorService,
      private http: HttpClient) {
      this.monthSelector.OnChange.subscribe(month => this.OnMonthChange(month));
      this.OnMonthChange(this.CurrentMonth);
   }

   private get CurrentMonth(): Month { return this.monthSelector.CurrentMonth; }
   public get ObserveTransfers(): Observable<TransferEntity[]> { return this.Cache.Observe; }

   private OnMonthChange(value: Month) {
      this.Cache.InitializeValue(value.ToCode());
      this.RefreshTransfers();
   }

   public async RefreshTransfers(): Promise<void> {
      try {
         this.busy.show();

         const url = `api/transfers/list/${this.CurrentMonth.Year}/${this.CurrentMonth.Month}`;
         const values = await this.http.get<TransferEntity[]>(url).toPromise();
         if (!values)
            return;

         this.Cache.SetTransfers(this.CurrentMonth.ToCode(), values);
      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async LoadTransfer(snapshot: ActivatedRouteSnapshot): Promise<TransferEntity> {
      try {
         this.busy.show();

         if (!snapshot || !snapshot.routeConfig || !snapshot.routeConfig.path)
            return null;

         if (snapshot.routeConfig.path == "new")
            return TransferEntity.Parse({ Date: this.monthSelector.DefaultDate });

         const transferID = snapshot.params?.transfer;
         if (!transferID)
            return null;

         let transfer = await this.http.get<TransferEntity>(`api/transfers/load/${transferID}`).toPromise();
         if (!transfer)
            return null;

         transfer = TransferEntity.Parse(transfer);
         return transfer;

      }
      catch { return null; /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async SaveTransfer(transfer: TransferEntity): Promise<boolean> {
      try {
         this.busy.show();

         if (!transfer)
            return false;

         if (transfer.TransferID == null)
            await this.http.post("api/transfers/insert", transfer).toPromise();
         else
            await this.http.put("api/transfers/update", transfer).toPromise();

         await this.RefreshTransfers();
         return true;

      }
      catch { return false; /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async RemoveTransfer(transfer: TransferEntity) {
      try {

         if (!transfer)
            return;

         const confirm = await this.message.Confirm("transfers.REMOVE_TEXT", "shared.REMOVE_CONFIRM_COMMAND", "shared.REMOVE_CANCEL_COMMAND");
         if (!confirm)
            return

         this.busy.show();

         await this.http.delete(`api/transfers/delete/${transfer.TransferID}`).toPromise();

         await this.RefreshTransfers();

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
