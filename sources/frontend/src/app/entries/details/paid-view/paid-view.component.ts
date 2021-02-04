import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-details-paid',
   templateUrl: './paid-view.component.html',
   styleUrls: ['./paid-view.component.scss']
})
export class PaidViewComponent implements OnInit {

   constructor() { }

   ngOnInit(): void {
      this.OnDataInit();
      this.OnFormInit();
   }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;

   private OnDataInit() {
      if (!this.data)
         return;
   }

   private OnFormInit() {
      if (!this.form)
         return;
      this.form.addControl("Paid", new FormControl(this.data?.Paid ?? false));
   }

}
