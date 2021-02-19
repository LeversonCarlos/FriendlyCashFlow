import { Component, OnInit } from '@angular/core';
import { BusyService } from '@elesse/shared';

@Component({
   selector: 'transfers-details-cancel',
   templateUrl: './cancel.component.html',
   styleUrls: ['./cancel.component.scss']
})
export class CancelComponent implements OnInit {

   constructor(private busy: BusyService) { }

   ngOnInit(): void {
   }

   public get IsBusy(): boolean { return this.busy.IsBusy; }

}
