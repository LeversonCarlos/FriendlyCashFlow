import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
   selector: 'accounts-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private route: ActivatedRoute) {
      this.paramID = this.route.snapshot.params.id;
   }

   public paramID: string;

   ngOnInit(): void {
   }

}
