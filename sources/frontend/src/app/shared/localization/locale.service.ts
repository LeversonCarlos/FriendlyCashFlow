import { Injectable, LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';

import localePt from '@angular/common/locales/pt';
import localeEn from '@angular/common/locales/en';

@Injectable({
   providedIn: 'root'
})
export class LocaleService {

   constructor() { }

   private availabledLanguages: string[] = ['pt-BR', 'en-US']
   private defaultLanguage: string = 'en-US'

   public get Language(): string {
      const currentLanguage = navigator.language || this.defaultLanguage
      if (this.availabledLanguages.includes(currentLanguage))
         return currentLanguage;
      else
         return this.defaultLanguage;
   }

   public RegisterLocale() {
      const language = this.Language;
      if (language == 'pt-BR')
         registerLocaleData(localePt);
      else
         registerLocaleData(localeEn);
   }

}

export const LocaleServiceProvider = {
   provide: LOCALE_ID,
   useFactory: (localeService: LocaleService) => {
      localeService.RegisterLocale();
      return localeService.Language;
   },
   deps: [LocaleService]
};
