import { Component, OnInit } from '@angular/core';
import { ResponsiveService } from '../../responsive/responsive.service';

@Component({
   selector: 'shared-authenticated-container',
   templateUrl: './authenticated-container.component.html',
   styleUrls: ['./authenticated-container.component.scss']
})
export class AuthenticatedContainerComponent implements OnInit {

   constructor(private responsive: ResponsiveService) { }

   public get IsMobile(): boolean { return this.responsive && this.responsive.IsMobile; }

   ngOnInit(): void {
   }

}
