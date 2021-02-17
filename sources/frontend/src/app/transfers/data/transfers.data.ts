import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { BusyService } from '@elesse/shared';
import { TransferEntity } from '../model/transfers.model';

@Injectable({
   providedIn: 'root'
})
export class TransfersData {

   constructor(
      private busy: BusyService,
      private http: HttpClient) { }

   @Output() OnDataChanged: EventEmitter<void> = new EventEmitter<void>();

   public async LoadTransfer(transferID: string): Promise<TransferEntity> {
      try {
         this.busy.show();

         if (!transferID)
            return null;

         if (transferID == 'new')
            return TransferEntity.Parse({});

         let value = await this.http.get<TransferEntity>(`api/transfers/load/${transferID}`).toPromise();
         if (!value)
            return null;

         value = TransferEntity.Parse(value);
         return value;

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

         this.OnDataChanged.emit();
         return true;

      }
      catch { return false; /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
