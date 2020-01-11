import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'src/app/shared/message/message.service';
import { ActivatedRoute } from '@angular/router';
import { Entry } from '../entries.viewmodels';
import { RelatedData } from 'src/app/shared/related-box/related-box.models';
import { Category, CategoriesService } from '../../categories/categories.service';
import { Account, AccountsService } from '../../accounts/accounts.service';
import { Pattern } from '../../patterns/patterns.viewmodels';
import { PatternsService } from '../../patterns/patterns.service';
import { Recurrency } from '../../recurrency/recurrency.viewmodels';

@Component({
   selector: 'fs-entry-details',
   templateUrl: './entry-details.component.html',
   styleUrls: ['./entry-details.component.scss']
})
export class EntryDetailsComponent implements OnInit {

   constructor(private service: EntriesService, private msg: MessageService,
      private categoryService: CategoriesService, private accountService: AccountsService, private patternService: PatternsService,
      private route: ActivatedRoute, private fb: FormBuilder) { }

   public Data: Entry;
   public inputForm: FormGroup;

   public async ngOnInit() {
      if (!await this.OnDataLoad()) { return; }
      this.OnFormCreate();
   }

   private async OnDataLoad(): Promise<boolean> {
      try {
         const paramID: string = this.route.snapshot.params.id;
         const paramType: string = this.route.snapshot.params.type;

         if (paramID == undefined && paramType != undefined) {
            this.Data = Object.assign(new Entry, {
               Type: paramType,
               Recurrency: new Recurrency(),
               DueDate: this.service.CurrentData.CurrentMonth,
               Active: true
            }); return true;
         }

         const entryID: number = Number(paramID);
         if (!entryID || entryID == 0) {
            this.msg.ShowWarning('ENTRIES_RECORD_NOT_FOUND_WARNING');
            this.service.showCurrentList();
            return false;
         }

         this.Data = await this.service.getEntry(entryID);
         if (!this.Data || this.Data.EntryID != entryID) {
            this.msg.ShowWarning('ENTRIES_RECORD_NOT_FOUND_WARNING');
            this.service.showCurrentList();
            return false;
         }

         if (this.Data.PatternRow) {
            this.PatternOptions = [this.OnPatternParse(this.Data.PatternRow)];
         }
         if (this.Data.AccountRow) {
            this.AccountOptions = [this.OnAccountParse(this.Data.AccountRow)];
         }
         if (this.Data.CategoryRow) {
            this.CategoryOptions = [this.OnCategoryParse(this.Data.CategoryRow)];
         }

         return true;
      }
      catch (ex) { console.error(ex) }
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Text: [this.Data.Text, Validators.required],
         PatternRow: [this.PatternOptions && this.PatternOptions.length ? this.PatternOptions[0] : null],
         AccountRow: [this.AccountOptions && this.AccountOptions.length ? this.AccountOptions[0] : null],
         EntryValue: [this.Data.EntryValue, [Validators.required, Validators.min(0.01)]],
         DueDate: [this.Data.DueDate, Validators.required],
         CategoryRow: [this.CategoryOptions && this.CategoryOptions.length ? this.CategoryOptions[0] : null],
         Paid: [this.Data.Paid],
         PayDate: [this.Data.PayDate]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         this.Data.EntryValue = values.EntryValue;
         this.Data.DueDate = values.DueDate;
         this.Data.Paid = values.Paid || false;
         this.Data.PayDate = values.PayDate;
      });
      this.inputForm.get("PatternRow").valueChanges.subscribe(opt => {
         this.Data.PatternID = null;
         if (opt && opt.id) {
            this.Data.PatternID = opt.id;
            this.CategoryOptions = [this.OnCategoryParse(opt.value.CategoryRow)];
            this.inputForm.get("Text").setValue(opt.value.Text);
            this.inputForm.get("CategoryRow").setValue(this.CategoryOptions[0]);
         }
      });
      this.inputForm.get("AccountRow").valueChanges.subscribe(value => {
         this.Data.AccountID = null;
         if (value && value.id) {
            this.Data.AccountID = value.id;
            const dueDateControl = this.inputForm.get("DueDate");
            dueDateControl.setValue(value.value.DueDate)
            dueDateControl.updateValueAndValidity();
         }
      });
      this.inputForm.get("CategoryRow").valueChanges.subscribe(opt => {
         this.Data.CategoryID = null;
         if (opt && opt.id) {
            this.Data.CategoryID = opt.id;
         }
      });
      this.inputForm.controls['Paid'].valueChanges.subscribe((paid) => this.OnPaidChanged(paid));
      this.OnPaidChanged(this.Data.Paid)
   }

   public PatternOptions: RelatedData<Pattern>[] = [];
   public async OnPatternChanging(val: string) {
      this.inputForm.get("Text").setValue(val);
      const patternList = await this.patternService.getPatterns(this.Data.Type, val);
      if (patternList == null) { return; }
      this.PatternOptions = patternList
         .map(item => this.OnPatternParse(item));
   }
   private OnPatternParse(item: Pattern): RelatedData<Pattern> {
      return Object.assign(new RelatedData, { id: item.PatternID, description: item.Text, badgeText: item.Count, value: item });
   }

   public AccountOptions: RelatedData<Account>[] = [];
   public async OnAccountChanging(val: string) {
      const accountList = await this.accountService.getAccounts(val);
      if (accountList == null) { return; }
      this.AccountOptions = accountList
         .map(item => this.OnAccountParse(item));
   }
   private OnAccountParse(item: Account): RelatedData<Account> {
      return Object.assign(new RelatedData, { id: item.AccountID, description: item.Text, value: item });
   }

   public CategoryOptions: RelatedData<Category>[] = [];
   public async OnCategoryChanging(val: string) {
      const categoryList = await this.categoryService.getCategories(this.Data.Type, val);
      if (categoryList == null) { return; }
      this.CategoryOptions = categoryList
         .map(item => this.OnCategoryParse(item));
   }
   private OnCategoryParse(item: Category): RelatedData<Category> {
      return Object.assign(new RelatedData, { id: item.CategoryID, description: item.HierarchyText, value: item });
   }

   private OnPaidChanged(paid: boolean) {
      const payDateControl = this.inputForm.controls['PayDate'];
      if (paid == true) {
         payDateControl.enable();
         payDateControl.setValidators([Validators.required]);
         payDateControl.setValue(this.Data.DueDate);
         payDateControl.markAsTouched();
      }
      else {
         payDateControl.clearValidators();
         payDateControl.markAsUntouched();
         payDateControl.setValue('');
         payDateControl.disable();
      }
      payDateControl.updateValueAndValidity();
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine) {
         if (!await this.msg.Confirm('BASE_CANCEL_CHANGES_CONFIRMATION_TEXT', 'BASE_CANCEL_CHANGES_CONFIRM', 'BASE_CANCEL_CHANGES_ABORT')) { return; }
      }
      this.service.showCurrentList();
   }

   public async OnSaveClick() {
      if (!await this.service.saveEntry(this.Data)) { return; }
      this.service.showCurrentList();
   }

   public async OnRemoveClick() {
      if (!await this.msg.Confirm('ENTRIES_REMOVE_CONFIRMATION_TEXT', 'BASE_REMOVE_COMMAND')) { return; }
      if (!await this.service.removeEntry(this.Data)) { return; }
      this.service.showCurrentList();
   }

}
