import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BusyService } from '@elesse/shared';

@Component({
   selector: 'transfers-details-confirm',
   templateUrl: './confirm.component.html',
   styleUrls: ['./confirm.component.scss']
})
export class ConfirmComponent implements OnInit {

   constructor(private busy: BusyService) { }

   ngOnInit(): void {
   }

   @Input() form: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

}
