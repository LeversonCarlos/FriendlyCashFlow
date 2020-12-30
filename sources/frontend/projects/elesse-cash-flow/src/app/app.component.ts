import { Component } from '@angular/core';
import { InsightsService } from '@elesse/shared';

@Component({
   selector: 'elesse-cash-flow-root',
   template: `
   <router-outlet></router-outlet>
   <shared-busy></shared-busy>
   `,
   styles: []
})
export class AppComponent {

   constructor(private insights: InsightsService) {
      this.insights.TrackEvent('Application Opened');
   }

}
