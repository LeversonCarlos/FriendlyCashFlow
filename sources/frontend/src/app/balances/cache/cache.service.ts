import { Injectable } from '@angular/core';
import { StorageService } from '@elesse/shared';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { BalanceEntity } from '../models/balance.model';

@Injectable({
   providedIn: 'root'
})
export class BalanceCache extends StorageService<string, BalanceEntity[]> {

   constructor() {
      super("BalanceCache");
   }

   private SubsKey: string;
   private Subs: Subscription;
   private Subject: BehaviorSubject<BalanceEntity[]>;
   public Observe: Observable<BalanceEntity[]>;

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

   public SetBalances(key: string, value: BalanceEntity[]) {
      const entries = value
         .map(entry => BalanceEntity.Parse(entry));
      this.SetValue(key, entries);
   }

}
