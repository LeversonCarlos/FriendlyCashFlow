import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { Pattern } from './patterns.viewmodels';
import { enCategoryType } from '../categories/categories.service';

@Injectable({
   providedIn: 'root'
})
export class PatternsService {

   constructor(private busy: BusyService,
      private http: HttpClient) { }

   // PATTERNS
   public async getPatterns(type: enCategoryType, searchText: string = ''): Promise<Pattern[]> {
      try {
         this.busy.show();
         let url = `api/patterns/search/${type}`;
         if (searchText) { url = `${url}/${encodeURIComponent(searchText)}`; }
         const dataList = await this.http.get<Pattern[]>(url)
            .pipe(map(items => items.map(item => Object.assign(new Pattern, item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // PATTERN
   public async getPattern(patternID: number): Promise<Pattern> {
      try {
         this.busy.show();
         const data = await this.http.get<Pattern>(`api/patterns/${patternID}`)
            .pipe(map(item => Object.assign(new Pattern, item)))
            .toPromise();
         return data;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

}
