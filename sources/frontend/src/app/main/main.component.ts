import { Component, OnInit } from '@angular/core';
import { InsightsService } from '@elesse/shared';

@Component({
   selector: 'elesse-root',
   template: `<router-outlet></router-outlet>`,
   styles: []
})
export class MainComponent implements OnInit {

   constructor(private insights: InsightsService) {
      this.insights.TrackEvent('Application Opened');
   }

   ngOnInit(): void {
   }

}
