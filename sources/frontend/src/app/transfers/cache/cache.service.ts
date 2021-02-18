import { Injectable } from '@angular/core';
import { StorageService } from '@elesse/shared';
import { TransferEntity } from '../model/transfers.model';

@Injectable({
   providedIn: 'root'
})
export class TransfersCache extends StorageService<string, TransferEntity[]> {

   constructor() {
      super("TransfersCache");
   }

}
