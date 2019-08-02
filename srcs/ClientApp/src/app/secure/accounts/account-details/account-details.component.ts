import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsService, Account } from '../accounts.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MessageService } from 'src/app/shared/message/message.service';

@Component({
   selector: 'fs-account-details',
   templateUrl: './account-details.component.html',
   styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {

   constructor(private service: AccountsService, private msg: MessageService,
      private route: ActivatedRoute, private fb: FormBuilder) { }

   public Data: Account;
   public inputForm: FormGroup;

   public async ngOnInit() {
      if (!await this.OnDataLoad()) { return; }
      this.OnFormCreate();
   }

   private async OnDataLoad(): Promise<boolean> {
      const paramID: string = this.route.snapshot.params.id;
      if (paramID == 'new') { this.Data = new Account(); return true; }

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
      return true;
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Text: [this.Data.Text, Validators.required],
         Type: [this.Data.Type, Validators.required],
         DueDay: [this.Data.DueDay],
         Active: [this.Data.Active, Validators.required]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         this.Data.Type = values.Type || 0;
         this.Data.DueDay = values.DueDay;
         this.Data.Active = values.Type || false;
      });
      this.inputForm.dirty
   }

   public async OnRemoveClick() {
      if (!await this.msg.Confirm('Do you want to remove this account?', 'Remove')) { return; }
      this.service.showList();
   }

   public OnCancelClick() {
      this.service.showList();
   }

   public OnSaveClick() {
      this.service.showList();
   }

}
