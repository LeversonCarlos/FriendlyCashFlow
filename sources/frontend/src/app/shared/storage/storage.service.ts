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

   public InitializeValues(...keys: KEY[]) {
      this._Keys = keys ?? [];
      this._Keys.forEach(key => {
         this._Data[`${key}`] = new BehaviorSubject<VALUE>(null)
         try {
            const valueString = localStorage.getItem(this.GetKey(key));
            if (valueString) {
               const value: VALUE = JSON.parse(valueString);
               if (value)
                  this._Data[`${key}`].next(value);
            }
         }
         catch { }
      });
   }

   public GetValue(key: KEY): Observable<VALUE> {
      return this._Data[`${key}`].asObservable();
   }

   public SetValue(key: KEY, value: VALUE): void {
      localStorage.setItem(this.GetKey(key), JSON.stringify(value))
      this._Data[`${key}`].next(value);
   }

}
