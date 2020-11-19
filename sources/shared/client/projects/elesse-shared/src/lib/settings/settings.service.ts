import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
   providedIn: 'root'
})
export class SettingsService {

   constructor(private http: HttpClient) { }

   private Settings: Observable<any> = null;

   private get SettingsFile(): string { return './assets/appsettings.json'; }

   public getSettings(): Observable<any> {
      if (this.Settings)
         return this.Settings
      else
         this.Settings = this.http.get<any>(this.SettingsFile);
      return this.Settings;
   }

}
