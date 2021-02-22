import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService } from '@elesse/shared';
import { TransfersData } from '../data/transfers.data';
import { TransferEntity } from '../model/transfers.model';
import { ControlNames } from './details.control-names';

@Component({
   selector: 'transfers-details',
   templateUrl: './details.component.html',
   styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {

   constructor(private transferData: TransfersData,
      private busy: BusyService, private msg: MessageService,
      private activatedRoute: ActivatedRoute, private router: Router, private fb: FormBuilder) { }

   public data: TransferEntity;
   public form: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   public async ngOnInit(): Promise<void> {
      this.data = await this.transferData.LoadTransfer(this.activatedRoute.snapshot);
      if (!this.data) {
         this.router.navigate(["/transactions/list"]);
         return;
      }
      this.form = this.fb.group({});
   }

   public OnAccountsChange() {
      const expenseAccount = this.form.get(ControlNames.ExpenseAccountRow);
      const incomeAccount = this.form.get(ControlNames.IncomeAccountRow);
      if (this.data.ExpenseAccountID && this.data.IncomeAccountID) {
         if (this.data.ExpenseAccountID == this.data.IncomeAccountID) {
            expenseAccount.setErrors({ sameAccount: true });
            expenseAccount.markAsTouched();
            incomeAccount.setErrors({ sameAccount: true });
            incomeAccount.markAsTouched();
            return;
         }
      }
      expenseAccount.setErrors(null);
      incomeAccount.setErrors(null);
   }

   public async OnCancelClick() {
      if (!this.form.pristine)
         if (!await this.msg.Confirm('shared.ROLLBACK_TEXT', 'shared.ROLLBACK_CONFIRM_COMMAND', 'shared.ROLLBACK_ABORT_COMMAND'))
            return;
      this.router.navigate(["/transactions/list"])
   }

   public async OnSaveClick() {
      if (!this.form.valid)
         return;
      if (!await this.transferData.SaveTransfer(this.data))
         return;
      this.router.navigate(["/transactions/list"])
   }

}
