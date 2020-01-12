import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';
import { Transfer } from '../entries.viewmodels';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RelatedData } from 'src/app/shared/related-box/related-box.models';
import { Account, AccountsService } from '../../accounts/accounts.service';

@Component({
   selector: 'fs-transfer-details',
   templateUrl: './transfer-details.component.html',
   styleUrls: ['./transfer-details.component.scss']
})
export class TransferDetailsComponent implements OnInit {

   constructor(private service: EntriesService,
      private accountService: AccountsService,
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
            this.Data = Object.assign(new Transfer, {
               TransferDate: this.service.CurrentData.CurrentMonth
            });
            return true;
         }

         // LOAD DATA
         /*
         const transferID: string = paramID;
         this.Data = await this.service.getEntry(entryID);
         if (!this.Data || this.Data.EntryID != entryID) {
            this.msg.ShowWarning('ENTRIES_RECORD_NOT_FOUND_WARNING');
            this.service.showCurrentList();
            return false;
         }
         */

         // RELATED DATA INIT
         if (this.Data.ExpenseAccountRow) {
            this.ExpenseAccountOptions = [this.OnExpenseAccountParse(this.Data.ExpenseAccountRow)];
         }

      }
      catch (ex) { console.error(ex) }
   }


   /* FORM: CREATE */
   public inputForm: FormGroup;
   private OnFormCreate() {
      this.inputForm = this.fb.group({
         ExpenseAccountRow: [this.ExpenseAccountOptions && this.ExpenseAccountOptions.length ? this.ExpenseAccountOptions[0] : null],
         TransferValue: [this.Data.TransferValue, [Validators.required, Validators.min(0.01)]],
         TransferDate: [this.Data.TransferDate, Validators.required],
      });

      this.inputForm.valueChanges.subscribe(values => this.OnFormChanged(values));
      this.inputForm.get("ExpenseAccountRow").valueChanges.subscribe(item => this.OnExpenseAccountChanged(item));
   }

   /* FORM: CHANGED */
   private OnFormChanged(values: any) {
      this.Data.TransferValue = values.TransferValue;
      this.Data.TransferDate = values.TransferDate;
   }


   /* EXPENSE ACCOUNT */
   public ExpenseAccountOptions: RelatedData<Account>[] = [];
   public async OnExpenseAccountChanging(val: string) {
      const dataList = await this.accountService.getAccounts(val);
      if (dataList == null) { return; }
      this.ExpenseAccountOptions = dataList
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


}
