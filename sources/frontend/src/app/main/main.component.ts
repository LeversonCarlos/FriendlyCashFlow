import { Component, OnInit } from '@angular/core';
import { InsightsService, LocalizationService } from '@elesse/shared';

@Component({
   selector: 'elesse-root',
   template: `<router-outlet></router-outlet>`,
   styles: []
})
export class MainComponent implements OnInit {

   constructor(private insights: InsightsService, private localization: LocalizationService) {
      this.insights.TrackEvent('Application Opened');
      this.localization.RefreshResources();
   }

   ngOnInit(): void {
   }

}
