import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RelatedData } from 'src/app/shared/related-box/related-box.models';
import { AccountsService } from '../../accounts/accounts.service';
import { Account } from '../../accounts/accounts.viewmodels';
import { Transfer } from '../../transfers/transfers.viewmodels';
import { TransfersService } from '../../transfers/transfers.service';
import { MessageService } from 'src/app/shared/message/message.service';

@Component({
   selector: 'fs-transfer-details',
   templateUrl: './transfer-details.component.html',
   styleUrls: ['./transfer-details.component.scss']
})
export class TransferDetailsComponent implements OnInit {

   constructor(private service: TransfersService,
      private entriesService: EntriesService, private accountService: AccountsService,
      private msg: MessageService,
      private route: ActivatedRoute, private fb: FormBuilder) { }


   /* INIT */
   public async ngOnInit() {
      try {
         if (!await this.OnDataLoad()) { return; }
         this.OnFormCreate();
      }
      catch (ex) { console.error(ex) }
   }


   /* DATA: LOAD */
   public Data: Transfer;
   private async OnDataLoad(): Promise<boolean> {
      try {
         const paramID: string = this.route.snapshot.params.id;

         // NEW MODEL
         if (paramID == 'new') {

            this.Data = Object.assign(new Transfer, {});

            // DEFAULT DATE
            const today = new Date();
               this.Data.TransferDate = (this.entriesService.CurrentData && this.entriesService.CurrentData.CurrentMonth) || today;
            if (this.Data.TransferDate.getFullYear() == today.getFullYear() && this.Data.TransferDate.getMonth() == today.getMonth()) { this.Data.TransferDate = today; }

            return true;
         }

         // LOAD DATA
         const transferID: string = paramID;
         this.Data = await this.service.getData(transferID);
         if (!this.Data || this.Data.TransferID != transferID) {
            this.msg.ShowWarning('TRANSFERS_RECORD_NOT_FOUND_WARNING');
            this.entriesService.showCurrentList();
            return false;
         }

         // RELATED DATA INIT
         if (this.Data.ExpenseAccountRow) {
            this.ExpenseAccountOptions = [this.OnExpenseAccountParse(this.Data.ExpenseAccountRow)];
         }
         if (this.Data.IncomeAccountRow) {
            this.IncomeAccountOptions = [this.OnIncomeAccountParse(this.Data.IncomeAccountRow)];
         }

         return true;
      }
      catch (ex) { console.error(ex) }
   }


   /* FORM: CREATE */
   public inputForm: FormGroup;
   private OnFormCreate() {
      this.inputForm = this.fb.group({
         ExpenseAccountRow: [this.ExpenseAccountOptions && this.ExpenseAccountOptions.length ? this.ExpenseAccountOptions[0] : null],
         IncomeAccountRow: [this.IncomeAccountOptions && this.IncomeAccountOptions.length ? this.IncomeAccountOptions[0] : null],
         TransferValue: [this.Data.TransferValue, [Validators.required, Validators.min(0.01)]],
         TransferDate: [this.Data.TransferDate, Validators.required],
      });

      this.inputForm.valueChanges.subscribe(values => this.OnFormChanged(values));
      this.inputForm.get("ExpenseAccountRow").valueChanges.subscribe(item => this.OnExpenseAccountChanged(item));
      this.inputForm.get("IncomeAccountRow").valueChanges.subscribe(item => this.OnIncomeAccountChanged(item));
   }

   /* FORM: CHANGED */
   private OnFormChanged(values: any) {
      this.Data.TransferValue = values.TransferValue;
      this.Data.TransferDate = values.TransferDate;
      if (this.Data.TransferDate) {
         let transferDate = new Date(this.Data.TransferDate);
         transferDate = new Date(transferDate.getFullYear(), transferDate.getMonth(), transferDate.getDate(), 12);
         this.Data.TransferDate = transferDate
      }
   }


   /* EXPENSE ACCOUNT */
   public ExpenseAccountOptions: RelatedData<Account>[] = [];
   public async OnExpenseAccountChanging(val: string) {
      let accountList = await this.accountService.getAccounts(val);
      if (accountList == null) { return; }
      accountList = accountList
         .filter(x => x.Active)
         .sort((a, b) => a.Text < b.Text ? -1 : 1);
      if (accountList.length == 0 && val == '') { this.msg.ShowInfo('ACCOUNTS_YOU_HAVE_NO_ACTIVE_ACCOUNTS_INFO'); }
      this.ExpenseAccountOptions = accountList
         .map(item => this.OnExpenseAccountParse(item));
   }
   private OnExpenseAccountChanged(item: RelatedData<Account>) {
      this.Data.ExpenseAccountID = null;
      if (item && item.id) {
         this.Data.ExpenseAccountID = item.id;
      }
   }
   private OnExpenseAccountParse(item: Account): RelatedData<Account> {
      return Object.assign(new RelatedData, { id: item.AccountID, description: item.Text, value: item });
   }


   /* INCOME ACCOUNT */
   public IncomeAccountOptions: RelatedData<Account>[] = [];
   public async OnIncomeAccountChanging(val: string) {
      let accountList = await this.accountService.getAccounts(val);
      if (accountList == null) { return; }
      accountList = accountList
         .filter(x => x.Active)
         .sort((a, b) => a.Text < b.Text ? -1 : 1);
      if (accountList.length == 0 && val == '') { this.msg.ShowInfo('ACCOUNTS_YOU_HAVE_NO_ACTIVE_ACCOUNTS_INFO'); }
      this.IncomeAccountOptions = accountList
         .map(item => this.OnIncomeAccountParse(item));
   }
   private OnIncomeAccountChanged(item: RelatedData<Account>) {
      this.Data.IncomeAccountID = null;
      if (item && item.id) {
         this.Data.IncomeAccountID = item.id;
      }
   }
   private OnIncomeAccountParse(item: Account): RelatedData<Account> {
      return Object.assign(new RelatedData, { id: item.AccountID, description: item.Text, value: item });
   }


   /* COMMANDS: CANCEL */
   public async OnCancelClick() {
      if (!this.inputForm.pristine) {
         if (!await this.msg.Confirm('BASE_CANCEL_CHANGES_CONFIRMATION_TEXT', 'BASE_CANCEL_CHANGES_CONFIRM', 'BASE_CANCEL_CHANGES_ABORT')) { return; }
      }
      this.entriesService.showCurrentList();
   }

   /* COMMANDS: SAVE */
   public async OnSaveClick() {
      if (!await this.service.saveData(this.Data)) { return; }
      this.entriesService.showCurrentList();
   }

   /* COMMANDS: REMOVE */
   public async OnRemoveClick() {
      if (!await this.msg.Confirm('TRANSFERS_REMOVE_CONFIRMATION_TEXT', 'BASE_REMOVE_COMMAND')) { return; }
      if (!await this.service.removeData(this.Data)) { return; }
      this.entriesService.showCurrentList();
   }


}
