import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusyService } from '@elesse/shared';
import { TransferEntity } from '../model/transfers.model';

@Injectable({
   providedIn: 'root'
})
export class TransfersData {

   constructor(
      private busy: BusyService,
      private http: HttpClient) { }

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

}
