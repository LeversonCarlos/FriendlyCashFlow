import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BusyService } from '@elesse/shared';

@Component({
   selector: 'transfers-details-commands-cancel',
   templateUrl: './cancel.component.html',
   styleUrls: ['./cancel.component.scss']
})
export class CancelComponent implements OnInit {

   constructor(private busy: BusyService) { }

   ngOnInit(): void {
   }

   @Input() form: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   @Output() click: EventEmitter<void> = new EventEmitter<void>();
   public OnClick() {
      this.click.emit();
   }

}
