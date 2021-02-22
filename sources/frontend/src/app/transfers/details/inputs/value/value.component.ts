import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TransferEntity } from 'src/app/transfers/model/transfers.model';
import { ControlNames } from '../../details.control-names';

@Component({
   selector: 'transfers-details-value',
   templateUrl: './value.component.html',
   styleUrls: ['./value.component.scss']
})
export class ValueComponent implements OnInit {

   constructor() { }

   @Input() data: TransferEntity;
   @Input() form: FormGroup;
   public formControlName: string = ControlNames.Value;

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
      this.form.addControl(ControlNames.Value, new FormControl(this.data.Value ?? null, [Validators.required, Validators.min(0.01)]));
      this.form.get(ControlNames.Value).valueChanges.subscribe((val: any) => {
         this.data.Value = val;
      });
   }

}
