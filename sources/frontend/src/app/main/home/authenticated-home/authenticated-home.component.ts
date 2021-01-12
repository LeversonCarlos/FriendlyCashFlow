import { Component, OnInit } from '@angular/core';
import { BusyService } from '@elesse/shared';

@Component({
   selector: 'elesse-authenticated-home',
   templateUrl: './authenticated-home.component.html',
   styleUrls: ['./authenticated-home.component.scss']
})
export class AuthenticatedHomeComponent implements OnInit {

   constructor(private busy: BusyService) { }

   public get IsBusy(): boolean { return this.busy.IsBusy; }

   ngOnInit(): void {
   }

   public SetBusy(state: boolean) {
      if (state)
         this.busy.show();
      else
         this.busy.hide();
   }

}
