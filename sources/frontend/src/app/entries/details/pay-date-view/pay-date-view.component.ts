import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-details-pay-date',
   templateUrl: './pay-date-view.component.html',
   styleUrls: ['./pay-date-view.component.scss']
})
export class PayDateViewComponent implements OnInit {

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
      this.form.addControl("PayDate", new FormControl(this.data?.PayDate ?? null));
      this.form.get("PayDate").valueChanges.subscribe((val: string) => {
         let payDate = new Date(val);
         payDate = new Date(payDate.getFullYear(), payDate.getMonth(), payDate.getDate(), 12);
         this.data.PayDate = payDate;
      });
   }

}
