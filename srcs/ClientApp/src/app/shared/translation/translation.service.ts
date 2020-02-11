import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DecimalPipe } from '@angular/common';

@Injectable({
   providedIn: 'root'
})
export class TranslationService {

   constructor(private http: HttpClient, private decimalPipe: DecimalPipe) { }

   public async getValue(key: string): Promise<string> {
      try {

         const storageKey: string = `Translation.${key}`;

         let result = localStorage.getItem(storageKey);
         if (result && result != '') { return result; }

         let response = await this.http
            .get<string[]>(`api/translations/${escape(key)}/`)
            .toPromise();

         if (response && response.length > 0) { result = response[0]; }
         if (result && result != '' && result != key) { localStorage.setItem(storageKey, result); }
         return result;
      }
      catch (ex) { console.error('translation', ex); return key; }
   }

   public getNumberFormat(value: number, decimalDigits: number = 0) {
      const format = `1.${decimalDigits}-${decimalDigits}`
      return this.decimalPipe.transform(value, format)
   }

}
