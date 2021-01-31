import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusyService } from '@elesse/shared';
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

}
