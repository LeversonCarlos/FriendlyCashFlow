import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { BusyService, MonthSelectorService } from '@elesse/shared';
import { TransferEntity } from '../model/transfers.model';

@Injectable({
   providedIn: 'root'
})
export class TransfersData {

   constructor(
      private busy: BusyService, private monthSelector: MonthSelectorService,
      private http: HttpClient) { }

   @Output() OnDataChanged: EventEmitter<void> = new EventEmitter<void>();

   public async LoadTransfer(snapshot: ActivatedRouteSnapshot): Promise<TransferEntity> {
      try {
         this.busy.show();

         if (!snapshot || !snapshot.routeConfig || !snapshot.routeConfig.path)
            return null;

         if (snapshot.routeConfig.path == "new")
            return this.GetNewTransfer();
         else
            return this.GetTransfer(snapshot.params?.transfer);

      }
      finally { this.busy.hide(); }
   }

   private GetNewTransfer(): TransferEntity {
      return TransferEntity.Parse({ Date: this.monthSelector.DefaultDate });
   }

   private async GetTransfer(transferID: string) {
      try {
         if (!transferID)
            return null;

         let transfer = await this.http.get<TransferEntity>(`api/transfers/load/${transferID}`).toPromise();
         if (!transfer)
            return null;

         transfer = TransferEntity.Parse(transfer);
         return transfer;
      }
      catch { return null; /* error absorber */ }
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

         this.OnDataChanged.emit();
         return true;

      }
      catch { return false; /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
