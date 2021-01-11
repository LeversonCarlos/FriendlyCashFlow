import { version } from 'package.json'
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { StorageService } from '../storage/storage.service';
import { TranslationData, TranslationValues } from './translation.data';

@Injectable({
   providedIn: 'root'
})
export class LocalizationService {

   constructor(private http: HttpClient) {
      this.Resources = new StorageService<string, TranslationValues>(`LocalizationService.${version}`);
      this.Resources.InitializeValues(...this.ResourceKeys);
   }

   private ResourceKeys: string[] = ["accounts"];
   private Resources: StorageService<string, TranslationValues>;

   public async RefreshResources(): Promise<void> {
      try {
         for (let index = 0; index < this.ResourceKeys.length; index++) {
            const resourceKey = this.ResourceKeys[index];

            const translationData = await this.http.get<TranslationData>(`api/${resourceKey}/translations`).toPromise();
            if (!translationData) continue;

            const translationKey = `${resourceKey}`;
            this.Resources.SetValue(translationKey, translationData.Values);
         }
      }
      catch { /* error absorber */ }
   }

   public GetTranslation(resource: string, key: string): string {
      try { return this.Resources.GetValue(resource)[key]; }
      catch { return `${resource}.${key}`; }
   }

}
