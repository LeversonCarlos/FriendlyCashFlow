import { Injectable } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';

@Injectable({
   providedIn: 'root'
})
export class ResponsiveService {

   constructor(private media: MediaMatcher) {
      this.mobileQuery = this.media.matchMedia('(max-width: 600px)');
   }

   private mobileQuery: MediaQueryList

   public get IsMobile(): boolean { return this.mobileQuery.matches; }

}
