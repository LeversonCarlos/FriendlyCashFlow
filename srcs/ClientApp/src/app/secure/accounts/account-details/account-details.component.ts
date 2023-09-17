import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsService } from '../accounts.service';
import { UntypedFormGroup, Validators, UntypedFormBuilder, UntypedFormControl } from '@angular/forms';
import { MessageService } from 'src/app/shared/message/message.service';
import { enAccountType, Account, AccountType } from '../accounts.viewmodels';

@Component({
   selector: 'fs-account-details',
   templateUrl: './account-details.component.html',
   styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {

   constructor(private service: AccountsService, private msg: MessageService,
      private route: ActivatedRoute, private fb: UntypedFormBuilder) { }

   public Data: Account;
   public AccountTypes: AccountType[];
   public inputForm: UntypedFormGroup;

   public async ngOnInit() {
      if (!await this.OnDataLoad()) { return; }
      this.OnFormCreate();
   }

   private async OnDataLoad(): Promise<boolean> {
      this.AccountTypes = await this.service.getAccountTypes();

      const paramID: string = this.route.snapshot.params.id;
      if (paramID == 'new') { this.Data = Object.assign(new Account, { Type: enAccountType.General, Active: true }); return true; }

      const accountID: number = Number(paramID);
      if (!accountID || accountID == 0) {
         this.msg.ShowWarning('ACCOUNTS_RECORD_NOT_FOUND_WARNING');
         this.service.showList();
         return false;
      }

      this.Data = await this.service.getAccount(accountID);
      if (!this.Data || this.Data.AccountID != accountID) {
         this.msg.ShowWarning('ACCOUNTS_RECORD_NOT_FOUND_WARNING');
         this.service.showList();
         return false;
      }

      return true;
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Text: [this.Data.Text, Validators.required],
         Type: [this.Data.Type, Validators.required],
         DueDay: new UntypedFormControl({ value: this.Data.DueDay, disabled: true }),
         ClosingDay: new UntypedFormControl({ value: this.Data.ClosingDay, disabled: true }),
         Active: [this.Data.Active]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         this.Data.Type = values.Type || enAccountType.General;
         this.Data.DueDay = values.DueDay;
         this.Data.ClosingDay = values.ClosingDay;
         this.Data.Active = values.Active || false;
      });
      this.inputForm.controls['Type'].valueChanges.subscribe((type) => {
         this.OnCreditCardChanged(type, 'ClosingDay');
         this.OnCreditCardChanged(type, 'DueDay');
      });
      this.OnCreditCardChanged(this.Data.Type, 'ClosingDay');
      this.OnCreditCardChanged(this.Data.Type, 'DueDay');
   }

   private OnCreditCardChanged(type: enAccountType, controlName: string) {
      const control = this.inputForm.controls[controlName];
      if (type == enAccountType.CreditCard) {
         control.enable();
         control.setValidators([Validators.required, Validators.min(1), Validators.max(31)]);
         control.markAsTouched();
      }
      else {
         control.clearValidators();
         control.markAsUntouched();
         control.setValue('');
         control.disable();
      }
      control.updateValueAndValidity();
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine) {
         if (!await this.msg.Confirm('BASE_CANCEL_CHANGES_CONFIRMATION_TEXT', 'BASE_CANCEL_CHANGES_CONFIRM', 'BASE_CANCEL_CHANGES_ABORT')) { return; }
      }
      this.service.showList();
   }

   public async OnSaveClick() {
      if (!await this.service.saveAccount(this.Data)) { return; }
      this.service.showList();
   }

   public async OnRemoveClick() {
      if (!await this.msg.Confirm('ACCOUNTS_REMOVE_CONFIRMATION_TEXT', 'BASE_REMOVE_COMMAND')) { return; }
      if (!await this.service.removeAccount(this.Data)) { return; }
      this.service.showList();
   }

}
