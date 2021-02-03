import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountEntity, AccountsData } from '@elesse/accounts';
import { RelatedData } from '@elesse/shared';
import { EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-details-account',
   templateUrl: './account-view.component.html',
   styleUrls: ['./account-view.component.scss']
})
export class AccountViewComponent implements OnInit {

   constructor(private accountsData: AccountsData) { }

   ngOnInit(): void {
      this.OnDataInit();
      this.OnFormInit();
   }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public AccountOptions: RelatedData<AccountEntity>[] = [];
   public AccountFiltered: RelatedData<AccountEntity>[] = [];

   private OnDataInit() {
      if (!this.data)
         return;
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
      this.form.addControl("AccountID", new FormControl(this.data?.AccountID ?? null));
      this.form.addControl("AccountRow", new FormControl(this.AccountFiltered?.length == 1 ? this.AccountFiltered[0] : null, Validators.required));
      this.form.get("AccountRow").valueChanges.subscribe((row: RelatedData<AccountEntity>) => {
         this.form.get("AccountID").setValue(row?.value?.AccountID ?? null);
      });
   }

   public async OnAccountChanging(val: string) {
      this.AccountFiltered = this.AccountOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

}
