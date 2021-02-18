import { Injectable } from '@angular/core';
import { StorageService } from '@elesse/shared';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { EntryEntity } from '../model/entries.model';

@Injectable({
   providedIn: 'root'
})
export class EntriesCache extends StorageService<string, EntryEntity[]> {

   constructor() {
      super("EntriesCache");
      this.Subject = new BehaviorSubject<EntryEntity[]>(null);
      this.Observe = this.Subject.asObservable();
   }

   private SubsKey: string;
   private Subs: Subscription;
   private Subject: BehaviorSubject<EntryEntity[]>;
   public Observe: Observable<EntryEntity[]>;

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

   public SetEntries(key: string, value: EntryEntity[]) {
      const entries = value
         .map(entry => EntryEntity.Parse(entry));
      this.SetValue(key, entries);
   }

}
