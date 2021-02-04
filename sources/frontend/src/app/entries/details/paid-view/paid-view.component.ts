import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
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
      this.form.get("Paid").valueChanges.subscribe((val: boolean) => {
         this.data.Paid = val;
         /*
      const payDateControl = this.inputForm.controls['PayDate'];
      if (paid == true) {
         const today = new Date();
         let payDate = new Date(this.Data.DueDate);
         if (today < payDate) { payDate = today; }
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
          */
      });
   }

}
