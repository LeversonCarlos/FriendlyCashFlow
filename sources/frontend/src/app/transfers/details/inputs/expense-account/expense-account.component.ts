import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TransferEntity } from 'src/app/transfers/model/transfers.model';

@Component({
   selector: 'transfers-details-inputs-expense-account',
   templateUrl: './expense-account.component.html',
   styleUrls: ['./expense-account.component.scss']
})
export class ExpenseAccountComponent implements OnInit {

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
      this.form.addControl("ExpenseAccountID", new FormControl(this.data.ExpenseAccountID ?? null, [Validators.required]));
      this.form.get("ExpenseAccountID").valueChanges.subscribe((val: any) => {
         this.data.ExpenseAccountID = val;
      });
   }

}
