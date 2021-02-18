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

   public InitializeValue(key: string) {
      super.InitializeValue(key);
      if (this.Subs) {
         if (this.SubsKey == key)
            return;
         this.Subs.unsubscribe();
      }
      this.Subs = this.GetObservable(key).subscribe(values => this.Subject.next(values));
      this.SubsKey = key;
   }

   public SetTransfers(key: string, value: TransferEntity[]) {
      const transfers = value
         .map(transfer => TransferEntity.Parse(transfer));
      this.SetValue(key, transfers);
   }

}
