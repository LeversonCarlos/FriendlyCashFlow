import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
   providedIn: 'root'
})
export class SettingsService {

   constructor(private http: HttpClient) { }

   private Settings: any = null;

   public async getSettings(): Promise<any> {
      if (!this.Settings)
         return this.Settings
      else
         this.Settings = await this.http.get<any>('./assets/appsettings.json').toPromise();
      return this.Settings;
   }

}
