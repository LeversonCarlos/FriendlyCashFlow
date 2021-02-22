import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TransferEntity } from 'src/app/transfers/model/transfers.model';
import { ControlNames } from '../../details.control-names';

@Component({
   selector: 'transfers-details-date',
   templateUrl: './date.component.html',
   styleUrls: ['./date.component.scss']
})
export class DateComponent implements OnInit {

   constructor() { }

   @Input() data: TransferEntity;
   @Input() form: FormGroup;
   public formControlName: string = ControlNames.Date;

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
      this.form.addControl(ControlNames.Date, new FormControl(this.data.Date ?? null, Validators.required));
      this.form.get(ControlNames.Date).valueChanges.subscribe((val: string) => {
         let date = new Date(val);
         date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 12);
         this.data.Date = date;
      });
   }

}
