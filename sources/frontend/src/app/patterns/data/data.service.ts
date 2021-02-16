import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enCategoryType } from '@elesse/categories';
import { BusyService } from '@elesse/shared';
import { Observable } from 'rxjs';
import { first, map } from 'rxjs/operators';
import { PatternsCache } from '../cache/cache.service';
import { PatternEntity } from '../model/patterns.model';

@Injectable({
   providedIn: 'root'
})
export class PatternsData {

   constructor(private Cache: PatternsCache,
      private busy: BusyService,
      private http: HttpClient) {
      this.RefreshPatterns();
   }

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

   public OnObservableFirstPush(type: enCategoryType, callback: (entities: PatternEntity[]) => void) {
      this.ObservePatterns(type)
         .pipe(first(entities => entities != null))
         .subscribe(entities => callback(entities));
   }

   public ObservePatterns = (type: enCategoryType): Observable<PatternEntity[]> =>
      this.Cache.GetObservable(type)
         .pipe(
            map(entries => entries?.map(entry => PatternEntity.Parse(entry)) ?? null)
         );

   public GetPatterns = (type: enCategoryType): PatternEntity[] =>
      this.Cache.GetValue(type);

}
