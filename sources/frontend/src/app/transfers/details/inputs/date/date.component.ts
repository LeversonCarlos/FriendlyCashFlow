import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TransferEntity } from 'src/app/transfers/model/transfers.model';

@Component({
   selector: 'transfers-details-inputs-date',
   templateUrl: './date.component.html',
   styleUrls: ['./date.component.scss']
})
export class DateComponent implements OnInit {

   constructor() { }

   @Input() data: TransferEntity;
   @Input() form: FormGroup;

   ngOnInit(): void {
      this.OnDataInit();
      this.OnFormInit();
   }

   private OnDataInit() {
      if (!this.data)
         return;
   }

   private OnFormInit() {
      if (!this.form)
         return;
      this.form.addControl("Date", new FormControl(this.data.Date ?? null, Validators.required));
      this.form.get("Date").valueChanges.subscribe((val: string) => {
         let date = new Date(val);
         date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 12);
         this.data.Date = date;
      });
   }

}
