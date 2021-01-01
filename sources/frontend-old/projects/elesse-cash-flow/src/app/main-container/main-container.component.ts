import { Component, OnInit } from '@angular/core';
import { InsightsService, ResponsiveService, TokenService } from '@elesse/shared';

@Component({
   selector: 'elesse-cash-flow-main-container',
   templateUrl: './main-container.component.html',
   styleUrls: ['./main-container.component.scss']
})
export class MainContainerComponent implements OnInit {

   constructor(private insights: InsightsService, private tokenService: TokenService, private responsive: ResponsiveService) {
      this.insights.TrackEvent('Application Opened');
   }

   public get IsAuthenticated(): boolean { return this.tokenService && this.tokenService.HasToken; }
   public get IsMobile(): boolean { return this.responsive && this.responsive.IsMobile; }

   ngOnInit(): void {
   }

}
