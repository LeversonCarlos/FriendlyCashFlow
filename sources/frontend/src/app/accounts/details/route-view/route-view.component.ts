import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountEntity } from '../../accounts.data';
import { AccountsService } from '../../accounts.service';

@Component({
   selector: 'accounts-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private service: AccountsService, private route: ActivatedRoute) { }

   public Data: AccountEntity

   async ngOnInit(): Promise<void> {
      const paramID = this.route.snapshot.params.id;
      this.Data = await this.service.LoadAccount(paramID);
   }

}
