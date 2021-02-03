import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService, RelatedData } from '@elesse/shared';
import { enCategoryType } from '@elesse/categories';
import { EntryEntity } from '../../model/entries.model';
import { EntriesData } from '../../data/entries.data';
import { AccountEntity, AccountsData } from '@elesse/accounts';

@Component({
   selector: 'entries-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private entriesData: EntriesData,
      private accountsData: AccountsData,
      private msg: MessageService, private busy: BusyService,
      private activatedRoute: ActivatedRoute, private router: Router, private fb: FormBuilder) { }

   public data: EntryEntity;
   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   private Type: enCategoryType;
   public get IsIncome(): boolean { return this.Type == enCategoryType.Income; }
   public get IsExpense(): boolean { return this.Type == enCategoryType.Expense; }

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;

      this.data = await this.entriesData.LoadEntry(paramID);
      if (!this.data) {
         this.router.navigate(["/entries/list"]);
         return;
      }

      this.Type = this.data.Pattern.Type;

      this.AccountOptions = this.accountsData.GetAccounts(true)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.AccountID,
            description: entity.Text,
            value: entity
         }));
      if (this.data.AccountID)
         this.AccountFiltered = this.AccountOptions
            .filter(entity => entity.value.AccountID == this.data.AccountID)

      this.OnFormCreate(this.data);
   }

   private OnFormCreate(data: EntryEntity) {
      this.inputForm = this.fb.group({
         Pattern: this.fb.group({
            Type: [data.Pattern.Type],
            Text: [data.Pattern.Text, Validators.required]
         }),
         AccountID: [data.AccountID],
         AccountRow: [this.AccountFiltered?.length == 1 ? this.AccountFiltered[0] : null, Validators.required],
         DueDate: [data.DueDate, Validators.required],
         EntryValue: [data.EntryValue, [Validators.required, Validators.min(0.01)]],
         Paid: [data.Paid],
         PayDate: [data.PayDate],
      });
      this.inputForm.get("AccountRow").valueChanges.subscribe((row: RelatedData<AccountEntity>) => {
         this.inputForm.get("AccountID").setValue(row?.value?.AccountID ?? null);
      });
   }

   public AccountOptions: RelatedData<AccountEntity>[] = [];
   public AccountFiltered: RelatedData<AccountEntity>[] = [];
   public async OnAccountChanging(val: string) {
      this.AccountFiltered = this.AccountOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine)
         if (!await this.msg.Confirm('shared.ROLLBACK_TEXT', 'shared.ROLLBACK_CONFIRM_COMMAND', 'shared.ROLLBACK_ABORT_COMMAND'))
            return;
      this.router.navigate(["/entries/list"])
   }

   public async OnSaveClick() {
      if (!this.inputForm.valid)
         return;
      const data: EntryEntity = Object.assign(new EntryEntity, this.inputForm.value);
      if (!await this.entriesData.SaveEntry(data))
         return;
      this.router.navigate(["/entries/list"])
   }

}
