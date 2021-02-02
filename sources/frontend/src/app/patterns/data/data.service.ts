import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enCategoryType } from '@elesse/categories';
import { BusyService } from '@elesse/shared';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PatternsCache } from '../cache/cache.service';
import { PatternEntity } from '../model/patterns.model';

@Injectable({
   providedIn: 'root'
})
export class PatternsData {

   constructor(private busy: BusyService,
      private http: HttpClient) {
      this.Cache = new PatternsCache();
   }

   private Cache: PatternsCache;

   public async RefreshPatterns(): Promise<void> {
      try {
         this.busy.show();

         const url = `api/patterns/list`;
         const values = await this.http.get<PatternEntity[]>(url).toPromise();
         if (!values)
            return;

         this.Cache.SetPatterns(values);
      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public ObservePatterns = (type: enCategoryType): Observable<PatternEntity[]> =>
      this.Cache.GetObservable(type)
         .pipe(
            map(entries => entries.map(entry => PatternEntity.Parse(entry)))
         );

   public GetPatterns = (type: enCategoryType): PatternEntity[] =>
      this.Cache.GetValue(type);

}
