import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
   providedIn: 'root'
})
export class StorageService<T> {

   constructor(name: string) {
      this._Name = name;
      this._Keys = [];
   }

   private readonly _Name: string;
   private _Keys: string[];
   private GetKey = (key: string): string => `StorageService.${this._Name}.${key}`;
   private _Data: { [id: string]: BehaviorSubject<T>; } = {};

   public InitializeValues(...keys: string[]) {
      this._Keys = keys ?? [];
      this._Keys.forEach(key => {
         this._Data[key] = new BehaviorSubject<T>(null)
         try {
            const valueString = localStorage.getItem(this.GetKey(key));
            if (valueString) {
               const value: T = JSON.parse(valueString);
               if (value)
                  this._Data[key].next(value);
            }
         }
         catch { }
      });
   }

   public GetValue(key: string): Observable<T> {
      return this._Data[key].asObservable();
   }

   public SetValue(key: string, value: T): void {
      localStorage.setItem(this.GetKey(key), JSON.stringify(value))
      this._Data[key].next(value);
   }

}
