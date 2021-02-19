import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountEntity, AccountsData } from '@elesse/accounts';
import { EntryEntity } from '@elesse/entries';
import { RelatedData } from '@elesse/shared';

@Component({
   selector: 'entries-details-account',
   templateUrl: './account.component.html',
   styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

   constructor(private accountsData: AccountsData) { }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public AccountOptions: RelatedData<AccountEntity>[] = [];
   public AccountFiltered: RelatedData<AccountEntity>[] = [];
   private FormControlID: string = "AccountID";
   public FormControlName: string = `${this.FormControlID}Row`;

   ngOnInit(): void {
      if (!this.data)
         return;
      this.accountsData.OnObservableFirstPush(() => {
         this.OnDataInit();
         this.OnFormInit();
      });
   }

   private OnDataInit() {
      this.AccountOptions = this.accountsData.GetAccounts(true)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.AccountID,
            description: entity.Text,
            value: entity
         }));
      if (this.data.AccountID)
         this.AccountFiltered = this.AccountOptions
            .filter(entity => entity.value.AccountID == this.data.AccountID)
   }

   private OnFormInit() {
      if (!this.form)
         return;
      this.form.addControl(this.FormControlID, new FormControl(this.data.AccountID ?? null));
      this.form.addControl(this.FormControlName, new FormControl(this.GetFirstAccount(), Validators.required));
      this.form.get(this.FormControlName).valueChanges.subscribe((row: RelatedData<AccountEntity>) => {
         this.data.AccountID = row?.value?.AccountID ?? null;
      });
   }

   public async OnAccountChanging(val: string) {
      this.AccountFiltered = this.AccountOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   private GetFirstAccount(): RelatedData<AccountEntity> {
      return this.AccountFiltered?.length == 1 ? this.AccountFiltered[0] : null;
   }

}
