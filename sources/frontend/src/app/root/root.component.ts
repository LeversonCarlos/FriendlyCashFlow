import { Component, OnInit } from '@angular/core';
import { InsightsService } from '@elesse/shared';

@Component({
   selector: 'elesse-root',
   templateUrl: './root.component.html',
   styleUrls: ['./root.component.scss']
})
export class RootComponent implements OnInit {

   constructor(private insights: InsightsService) {
      this.insights.TrackEvent('Application Opened');
   }

   title = 'CashFlow';

   ngOnInit(): void {
   }

}
