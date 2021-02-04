import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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
      this.form.get("Paid").valueChanges.subscribe(x => this.OnPaidChanged(x));
      this.OnPaidChanged(this.data.Paid, false);
   }

   private OnPaidChanged(val: boolean, setValue: boolean = true) {
      this.data.Paid = val;
      const payDateControl = this.form.get("PayDate");

      if (this.data.Paid) {
         const today = new Date();
         let payDate = new Date(this.data.DueDate);
         if (today < payDate)
            payDate = today;
         payDateControl.enable();
         payDateControl.setValidators([Validators.required]);
         if (setValue) { payDateControl.setValue(payDate); }
         payDateControl.markAsTouched();
      }
      else {
         payDateControl.clearValidators();
         payDateControl.markAsUntouched();
         if (setValue) { payDateControl.setValue(''); }
         payDateControl.disable();
      }

      payDateControl.updateValueAndValidity();
   }

}
