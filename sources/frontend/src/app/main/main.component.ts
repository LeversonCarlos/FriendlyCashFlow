import { Component, OnInit } from '@angular/core';
import { InsightsService } from '@elesse/shared';
import { MainData } from './data/data.service';

@Component({
   selector: 'elesse-root',
   template: `<router-outlet></router-outlet>`,
   styles: []
})
export class MainComponent implements OnInit {

   constructor(private mainData: MainData, private insights: InsightsService) {
      this.insights.TrackEvent('Application Opened');
   }

   ngOnInit(): void {
      this.mainData.RefreshAll();
   }

}
