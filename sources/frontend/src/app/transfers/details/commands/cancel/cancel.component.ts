import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
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

   @Input() form: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

}
