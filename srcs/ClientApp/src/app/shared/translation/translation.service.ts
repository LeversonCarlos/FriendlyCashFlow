import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
   providedIn: 'root'
})
export class TranslationService {

   constructor(private http: HttpClient) { }

   public async getValue(val: string): Promise<string> {
      try {
         const result = await this.http.get<string>(`api/translations/${val}`)
            .toPromise();
         return result;
      }
      catch{ return val; }
   }

}
