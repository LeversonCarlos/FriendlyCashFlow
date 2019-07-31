import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
   selector: 'fs-account-details',
   templateUrl: './account-details.component.html',
   styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {

   constructor(private route: ActivatedRoute) { }

   public ngOnInit() {
      this.route.params.subscribe(params => console.log(params.id));
   }

}
