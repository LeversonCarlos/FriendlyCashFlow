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
      this.Resources.PersistentStorage = false;
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

            this.Resources.SetValue(resourceKey, translationData.Values);
         }
      }
      catch { /* error absorber */ }
   }

   public async GetTranslationAsync(value: string): Promise<string> {
      try {
         const valueParts = value.split(".");
         const resourceKey = valueParts[0];
         const translationKey = valueParts[1];

         let resourceData = this.Resources.GetValue(resourceKey);
         if (resourceData == undefined || resourceData == null) {
            const translationData = await this.http.get<TranslationData>(`api/${resourceKey}/translations`).toPromise();
            if (translationData) {
               this.Resources.SetValue(resourceKey, translationData.Values);
               resourceData = this.Resources.GetValue(resourceKey);
            }
         }

         if (resourceData == undefined || resourceData == null)
            return value;

         const translationValue = resourceData[translationKey];
         if (translationValue)
            return translationValue;

         return value;
      }
      catch { return value; }
   }

   public GetTranslation(value: string): string {
      try {
         const keyParts = value.split(".");
         const resource = keyParts[0];
         const key = keyParts[1];
         const resourceData = this.Resources.GetValue(resource);
         console.log(value, resourceData)
         const result = resourceData[key];
         if (result)
            return result;
         return value;
      }
      catch { return value; }
   }

}
