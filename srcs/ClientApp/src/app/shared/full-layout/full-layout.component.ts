import { Component, Input, OnInit } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';

@Component({
   selector: 'fs-full-layout',
   templateUrl: './full-layout.component.html',
   styleUrls: ['./full-layout.component.scss']
})
export class FullLayoutComponent implements OnInit {

   constructor(private media: MediaMatcher) {
      this.mobileQuery = this.media.matchMedia('(max-width: 960px)');
   }

   public mobileQuery: MediaQueryList

   @Input() IsPrinting: boolean

   public ngOnInit() {
   }

}
