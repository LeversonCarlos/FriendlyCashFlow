import { Injectable } from '@angular/core';
import { StorageService } from '@elesse/shared';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { TransferEntity } from '../model/transfers.model';

@Injectable({
   providedIn: 'root'
})
export class TransfersCache extends StorageService<string, TransferEntity[]> {

   constructor() {
      super("TransfersCache");
      this.Subject = new BehaviorSubject<TransferEntity[]>(null);
      this.Observe = this.Subject.asObservable();
   }

   private SubsKey: string;
   private Subs: Subscription;
   private Subject: BehaviorSubject<TransferEntity[]>;
   public Observe: Observable<TransferEntity[]>;

}
