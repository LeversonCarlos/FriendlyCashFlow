import { Component, OnInit } from '@angular/core';
import { Router, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';
import { BusyService } from '../busy/busy.service';

@Component({
   selector: 'fs-lazy-loading',
   templateUrl: './lazy-loading.component.html',
   styleUrls: ['./lazy-loading.component.scss']
})
export class LazyLoadingComponent implements OnInit {

   constructor(private router: Router, private busy: BusyService) { }

   public ngOnInit() {
      this.router.events.subscribe(event => {
         if (event instanceof RouteConfigLoadStart) {
            this.busy.show();
         } else if (event instanceof RouteConfigLoadEnd) {
            this.busy.hide();
         }
      });
   }

}
