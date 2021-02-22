import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EntryEntity } from 'src/app/entries/model/entries.model';
import { ControlNames } from '../../details.control-names';

@Component({
   selector: 'entries-details-pay-date',
   templateUrl: './pay-date.component.html',
   styleUrls: ['./pay-date.component.scss']
})
export class PayDateComponent implements OnInit {

   constructor() { }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public formControlName: string = ControlNames.PayDate;

   ngOnInit(): void {
      this.OnDataInit();
      this.OnFormInit();
   }

   private OnDataInit() {
      if (!this.data)
         return;
   }

   private OnFormInit() {
      if (!this.form || !this.data)
         return;
      this.form.addControl(ControlNames.PayDate, new FormControl(this.data.PayDate ?? null));
      this.form.get(ControlNames.PayDate).valueChanges.subscribe((val: string) => {
         let payDate = new Date(val);
         payDate = new Date(payDate.getFullYear(), payDate.getMonth(), payDate.getDate(), 12);
         this.data.PayDate = payDate;
      });
      this.form.get(ControlNames.Paid).valueChanges.subscribe(x => this.OnPaidChanged(x));
      this.OnPaidChanged(this.data.Paid, false);
   }

   private OnPaidChanged(val: boolean, setValue: boolean = true) {
      this.data.Paid = val;
      const payDateControl = this.form.get(ControlNames.PayDate);

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
