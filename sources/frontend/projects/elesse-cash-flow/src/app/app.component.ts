import { Component } from '@angular/core';
import { InsightsService } from '@elesse/shared';

@Component({
   selector: 'elesse-cash-flow-root',
   template: `<elesse-cash-flow-home></elesse-cash-flow-home>`,
   styles: []
})
export class AppComponent {

   constructor(private insights: InsightsService) {
      this.insights.TrackEvent('Application Opened');
   }

}
