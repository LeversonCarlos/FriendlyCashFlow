import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { reduce } from 'rxjs/operators';

@Injectable({
   providedIn: 'root'
})
export class TranslationService {

   constructor(private http: HttpClient) { }

   public async getValue(val: string): Promise<string> {
      try {
         const result = await this.http.get<string[]>(`api/translations/${val}`)
            .toPromise();
         if (result && result.length == 1) { return result[0]; }
         return val;
      }
      catch (ex) { console.error('translation', ex); return val; }
   }

}
