import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService, RelatedData } from '@elesse/shared';
import { CategoriesData, CategoryEntity, enCategoryType } from '@elesse/categories';
import { EntryEntity } from '../../model/entries.model';
import { EntriesData } from '../../data/entries.data';
import { AccountEntity, AccountsData } from '@elesse/accounts';
import { PatternEntity, PatternsData } from '@elesse/patterns';

@Component({
   selector: 'entries-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private entriesData: EntriesData,
      private accountsData: AccountsData, private categoriesData: CategoriesData, private patternsData: PatternsData,
      private msg: MessageService, private busy: BusyService,
      private activatedRoute: ActivatedRoute, private router: Router, private fb: FormBuilder) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   private Type: enCategoryType;
   public get IsIncome(): boolean { return this.Type == enCategoryType.Income; }
   public get IsExpense(): boolean { return this.Type == enCategoryType.Expense; }

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;

      const data = await this.entriesData.LoadEntry(paramID);
      if (!data)
         this.router.navigate(["/entries/list"])

      this.Type = data.Pattern.Type;

      this.PatternOptions = this.patternsData.GetPatterns(data.Pattern.Type)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.PatternID,
            description: entity.Text,
            value: entity
         }));
      if (data.Pattern.PatternID)
         this.PatternFiltered = this.PatternOptions
            .filter(entity => entity.value.PatternID == data.Pattern.PatternID)

      this.CategoryOptions = this.categoriesData.GetCategories(data.Pattern.Type)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.CategoryID,
            description: entity.HierarchyText,
            value: entity
         }));
      if (data.Pattern.CategoryID)
         this.CategoryFiltered = this.CategoryOptions
            .filter(entity => entity.value.CategoryID == data.Pattern.CategoryID)

      this.AccountOptions = this.accountsData.GetAccounts(true)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.AccountID,
            description: entity.Text,
            value: entity
         }));
      if (data.AccountID)
         this.AccountFiltered = this.AccountOptions
            .filter(entity => entity.value.AccountID == data.AccountID)

      this.OnFormCreate(data);
   }

   private OnFormCreate(data: EntryEntity) {
      this.inputForm = this.fb.group({
         Pattern: this.fb.group({
            PatternID: [data.Pattern.PatternID],
            PatternRow: [this.PatternFiltered?.length == 1 ? this.PatternFiltered[0] : null, Validators.required],
            Type: [data.Pattern.Type],
            CategoryID: [data.Pattern.CategoryID],
            CategoryRow: [this.CategoryFiltered?.length == 1 ? this.CategoryFiltered[0] : null, Validators.required],
            Text: [data.Pattern.Text, Validators.required]
         }),
         AccountID: [data.AccountID],
         AccountRow: [this.AccountFiltered?.length == 1 ? this.AccountFiltered[0] : null, Validators.required],
         DueDate: [data.DueDate, Validators.required],
         EntryValue: [data.EntryValue, [Validators.required, Validators.min(0.01)]],
         Paid: [data.Paid],
         PayDate: [data.PayDate],
      });
      this.inputForm.get("Pattern").get("PatternRow").valueChanges.subscribe((row: RelatedData<PatternEntity>) => {
         this.inputForm.get("Pattern").get("PatternID").setValue(row?.value?.PatternID ?? null);
         this.inputForm.get("Pattern").get("Text").setValue(row?.value?.Text ?? null);
         this.inputForm.get("Pattern").get("CategoryID").setValue(row?.value?.CategoryID ?? null);
         this.CategoryFiltered = this.CategoryOptions
            .filter(entity => entity.value.CategoryID == row?.value?.CategoryID ?? null)
         this.inputForm.get("Pattern").get("CategoryRow").setValue(this.CategoryFiltered?.length == 1 ? this.CategoryFiltered[0] : null);
      });
      this.inputForm.get("Pattern").get("CategoryRow").valueChanges.subscribe((row: RelatedData<CategoryEntity>) => {
         this.inputForm.get("Pattern").get("CategoryID").setValue(row?.value?.CategoryID ?? null);
      });
      this.inputForm.get("AccountRow").valueChanges.subscribe((row: RelatedData<AccountEntity>) => {
         this.inputForm.get("AccountID").setValue(row?.value?.AccountID ?? null);
      });
   }

   public PatternOptions: RelatedData<PatternEntity>[] = [];
   public PatternFiltered: RelatedData<PatternEntity>[] = [];
   public async OnPatternChanging(val: string) {
      this.PatternFiltered = this.PatternOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   public CategoryOptions: RelatedData<CategoryEntity>[] = [];
   public CategoryFiltered: RelatedData<CategoryEntity>[] = [];
   public async OnCategoryChanging(val: string) {
      this.CategoryFiltered = this.CategoryOptions
         .filter(entity => entity.value.HierarchyText.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
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
