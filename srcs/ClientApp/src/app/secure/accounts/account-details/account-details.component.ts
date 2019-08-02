import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsService, Account, enAccountType } from '../accounts.service';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { MessageService } from 'src/app/shared/message/message.service';
import { SelectData } from 'src/app/shared/common/common.models';

@Component({
   selector: 'fs-account-details',
   templateUrl: './account-details.component.html',
   styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {

   constructor(private service: AccountsService, private msg: MessageService,
      private route: ActivatedRoute, private fb: FormBuilder) { }

   public Data: Account;
   public AccountTypes: SelectData<enAccountType>[];
   public inputForm: FormGroup;

   public async ngOnInit() {
      if (!await this.OnDataLoad()) { return; }
      this.OnFormCreate();
   }

   private async OnDataLoad(): Promise<boolean> {
      const paramID: string = this.route.snapshot.params.id;
      if (paramID == 'new') { this.Data = Object.assign(new Account, { Active: true }); return true; }

      const accountID: number = Number(paramID);
      if (!accountID || accountID == 0) {
         this.msg.ShowWarning('Account record not found to be displayed!');
         this.service.showList();
         return false;
      }

      this.Data = await this.service.getAccount(accountID);
      if (!this.Data || this.Data.AccountID != accountID) {
         this.msg.ShowWarning('Account record not found to be displayed!');
         this.service.showList();
         return false;
      }

      this.AccountTypes = this.service.getAccountTypes();
      return true;
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Text: [this.Data.Text, Validators.required],
         Type: [this.Data.Type, Validators.required],
         DueDay: new FormControl({ value: this.Data.DueDay, disabled: true }),
         Active: [this.Data.Active]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         this.Data.Type = values.Type || enAccountType.General;
         this.Data.DueDay = values.DueDay;
         this.Data.Active = values.Active || false;
      });
      this.inputForm.controls['Type'].valueChanges.subscribe((type) => this.OnCreditCardChanged(type, 'DueDay'));
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

   public async OnRemoveClick() {
      if (!await this.msg.Confirm('Do you want to remove this account?', 'Remove')) { return; }
      if (!await this.service.removeAccount(this.Data)) { return; }
      this.service.showList();
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine) {
         if (!await this.msg.Confirm('Do you want to cancel and lose changes?', 'Cancel Anyway', 'Go Back')) { return; }
      }
      this.service.showList();
   }

   public async OnSaveClick() {
      if (!await this.service.saveAccount(this.Data)) { return; }
      this.service.showList();
   }

}
