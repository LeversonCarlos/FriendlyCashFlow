import { Injectable, LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';

/* REGISTER PT-BR LOCALE */
import localePt from '@angular/common/locales/pt';
registerLocaleData(localePt);

@Injectable({
   providedIn: 'root'
})
export class LocaleService {

   constructor() { }

   private availabledLanguages: string[] = ['pt-BR', 'en-US']
   private defaultLanguage: string = 'en-US'

   public get Language(): string {
      const currentLanguage = navigator.language || this.defaultLanguage
      if (this.availabledLanguages.includes(currentLanguage)) { return currentLanguage }
      else { return this.defaultLanguage }
   }

}

export const LocaleServiceProvider = {
   provide: LOCALE_ID,
   useFactory: (localeService: LocaleService) => {
      return localeService.Language;
   },
   deps: [LocaleService]
};
