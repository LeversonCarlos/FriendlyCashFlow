import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
   providedIn: 'root'
})
export class TranslationService {

   constructor(private http: HttpClient) { }

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

}
