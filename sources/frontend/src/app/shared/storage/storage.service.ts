import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
   providedIn: 'root'
})
export class StorageService<KEY, VALUE> {

   constructor(name: string) {
      this._Name = name;
      this._Keys = [];
   }

   private readonly _Name: string;
   private _Data: { [id: string]: BehaviorSubject<VALUE>; } = {};

   private _Keys: KEY[];
   private GetKey = (key: KEY): string => `StorageService.${this._Name}.${key}`;

   public PersistentStorage: boolean = true;

   public InitializeValues(...keys: KEY[]) {
      this._Keys = keys ?? [];
      this._Keys.forEach(key => {
         this.InitializeValue(key);
      });
   }

   public InitializeValue(key: KEY) {
      if (!this._Keys.includes(key))
         this._Keys.push(key);
      this._Data[`${key}`] = new BehaviorSubject<VALUE>(null)
      if (this.PersistentStorage)
         try {
            const valueString = localStorage.getItem(this.GetKey(key));
            if (valueString) {
               const value: VALUE = JSON.parse(valueString);
               if (value)
                  this._Data[`${key}`].next(value);
            }
         }
         catch { }
   }

   public GetObservable(key: KEY): Observable<VALUE> {
      return this._Data[`${key}`].asObservable();
   }

   public GetValue(key: KEY): VALUE {
      return this._Data[`${key}`].getValue();
   }

   public SetValue(key: KEY, value: VALUE): void {
      localStorage.setItem(this.GetKey(key), JSON.stringify(value))
      this._Data[`${key}`].next(value);
   }

}
