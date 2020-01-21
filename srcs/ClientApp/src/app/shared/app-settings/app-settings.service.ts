import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

class AppSettingsJson {
   AppSettings: any
}

@Injectable({
   providedIn: 'root'
})
export class AppSettingsService {

   constructor(private http: HttpClient) { }

   private appSettings: Observable<any> = null;
   getSettings(): Observable<any> {
      if (!this.appSettings) { return this.appSettings }
      else {
         this.appSettings = this.http.get<AppSettingsJson>('./assets/appsettings.json')
            .pipe(map(data => data.AppSettings));
         return this.appSettings;
      }
   }

}
