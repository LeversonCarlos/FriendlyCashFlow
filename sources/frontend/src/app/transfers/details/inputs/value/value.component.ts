import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TransferEntity } from 'src/app/transfers/model/transfers.model';

@Component({
   selector: 'transfers-details-inputs-value',
   templateUrl: './value.component.html',
   styleUrls: ['./value.component.scss']
})
export class ValueComponent implements OnInit {

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
      this.form.addControl("Value", new FormControl(this.data.Value ?? null, [Validators.required, Validators.min(0.01)]));
      this.form.get("Value").valueChanges.subscribe((val: any) => {
         this.data.Value = val;
      });
   }

}
