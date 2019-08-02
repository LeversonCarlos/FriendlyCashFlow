import { Injectable } from '@angular/core';

@Injectable({
   providedIn: 'root'
})
export class TranslationService {

   constructor() { }

   public async getValue(val: string): Promise<string> {
      return val;
   }

}
