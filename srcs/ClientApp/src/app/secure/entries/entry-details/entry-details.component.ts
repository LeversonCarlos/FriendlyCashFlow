import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { MessageService } from 'src/app/shared/message/message.service';
import { ActivatedRoute } from '@angular/router';
import { Entry } from '../entries.viewmodels';
import { RelatedData } from 'src/app/shared/related-box/related-box.models';
import { Category, enCategoryType } from '../../categories/categories.viewmodels';
import { CategoriesService } from '../../categories/categories.service';
import { AccountsService } from '../../accounts/accounts.service';
import { Account } from '../../accounts/accounts.viewmodels';
import { Pattern } from '../../patterns/patterns.viewmodels';
import { PatternsService } from '../../patterns/patterns.service';
import { Recurrency, enRecurrencyType } from '../../recurrency/recurrency.viewmodels';
import { EnumVM } from 'src/app/shared/common/common.models';
import { RecurrencyService } from '../../recurrency/recurrency.service';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';

@Component({
   selector: 'fs-entry-details',
   templateUrl: './entry-details.component.html',
   styleUrls: ['./entry-details.component.scss']
})
export class EntryDetailsComponent implements OnInit {

   constructor(private service: EntriesService, private msg: MessageService,
      private categoryService: CategoriesService, private accountService: AccountsService, private patternService: PatternsService, private recurrencyService: RecurrencyService,
      private appInsights: AppInsightsService,
      private route: ActivatedRoute, private fb: FormBuilder) { }


   /* INIT */
   public async ngOnInit() {
      try {
         this.RecurrencyTypes = await this.recurrencyService.getRecurrencyTypes();
         if (!await this.OnDataLoad()) { return; }
         this.OnFormCreate();
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex) }
   }


   /* DATA: LOAD */
   public Data: Entry;
   private defaultDueDate: Date
   private async OnDataLoad(): Promise<boolean> {
      try {
         const paramID: string = this.route.snapshot.params.id;
         const paramType: string = this.route.snapshot.params.type;

         // DEFAULT DUE DATE
         const today = new Date();
         this.defaultDueDate = (this.service.CurrentData && this.service.CurrentData.CurrentMonth) || today;
         if (this.defaultDueDate.getFullYear() == today.getFullYear() && this.defaultDueDate.getMonth() == today.getMonth()) { this.defaultDueDate = today; }

         // NEW MODEL
         if (paramID == undefined && paramType != undefined) {

            this.Data = Object.assign(new Entry, {
               Type: (paramType == 'Income' ? enCategoryType.Income : enCategoryType.Expense),
               Recurrency: new Recurrency,
               DueDate: this.defaultDueDate,
               Active: true
            });

            if (this.service.CurrentData.CurrentAccount > 0) {
               this.Data.AccountID = this.service.CurrentData.CurrentAccount;
               this.Data.AccountRow = await this.accountService.getAccount(this.Data.AccountID);
               if (this.Data.AccountRow) {
                  this.Data.AccountText = this.Data.AccountRow.Text
                  if (this.Data.AccountRow.DueDate) { this.Data.DueDate = this.Data.AccountRow.DueDate }
                  this.AccountOptions = [this.OnAccountParse(this.Data.AccountRow)];
               }
            }

            return true;
         }

         // LOAD DATA
         const entryID: number = Number(paramID);
         this.Data = await this.service.getEntry(entryID);
         if (!this.Data || this.Data.EntryID != entryID) {
            this.msg.ShowWarning('ENTRIES_RECORD_NOT_FOUND_WARNING');
            this.service.showCurrentList();
            return false;
         }

         // RELATED DATA INIT
         if (this.Data.PatternRow) {
            this.PatternOptions = [this.OnPatternParse(this.Data.PatternRow)];
         }
         if (this.Data.AccountRow) {
            this.AccountOptions = [this.OnAccountParse(this.Data.AccountRow)];
         }
         if (this.Data.CategoryRow) {
            this.CategoryOptions = [this.OnCategoryParse(this.Data.CategoryRow)];
         }

         // RESULT
         return true;
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex) }
   }



   /* FORM: CREATE */
   public inputForm: FormGroup;
   private OnFormCreate() {
      try {
         this.inputForm = this.fb.group({
            Text: [this.Data.Text, Validators.required],
            PatternRow: [this.PatternOptions && this.PatternOptions.length ? this.PatternOptions[0] : null],
            AccountRow: [this.AccountOptions && this.AccountOptions.length ? this.AccountOptions[0] : null],
            EntryValue: [this.Data.EntryValue, [Validators.required, Validators.min(0.01)]],
            DueDate: [this.Data.DueDate, Validators.required],
            CategoryRow: [this.CategoryOptions && this.CategoryOptions.length ? this.CategoryOptions[0] : null],
            Paid: [this.Data.Paid],
            PayDate: [this.Data.PayDate],
            RecurrencyActivate: [false],
            RecurrencyType: [this.Data.Recurrency && this.Data.Recurrency.Type],
            RecurrencyCount: [this.Data.Recurrency && this.Data.Recurrency.Count]
         });

         this.inputForm.valueChanges.subscribe(values => this.OnFormChanged(values));
         this.inputForm.get("PatternRow").valueChanges.subscribe(item => this.OnPatternChanged(item));
         this.inputForm.get("AccountRow").valueChanges.subscribe(item => this.OnAccountChanged(item));
         this.inputForm.get("CategoryRow").valueChanges.subscribe(item => this.OnCategoryChanged(item));
         this.inputForm.get('RecurrencyActivate').valueChanges.subscribe((activate) => this.OnRecurrencyActivateChanged(activate));
         this.inputForm.get('Paid').valueChanges.subscribe((paid) => this.OnPaidChanged(paid));

         this.OnPaidChanged(this.Data.Paid, false)

      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex); }
   }

   /* FORM: CHANGED */
   private OnFormChanged(values: any) {
      this.Data.Text = values.Text || '';
      this.Data.EntryValue = values.EntryValue;
      this.Data.DueDate = values.DueDate;
      this.Data.Paid = values.Paid || false;
      this.Data.PayDate = values.PayDate;
      if (this.Data.Recurrency) {
         this.Data.Recurrency.Type = values.RecurrencyType
         this.Data.Recurrency.Count = values.RecurrencyCount
      }
   }



   /* PATTERN */
   public PatternOptions: RelatedData<Pattern>[] = [];
   public async OnPatternChanging(val: string) {
      try {
         this.inputForm.get("Text").setValue(val);
         const patternList = await this.patternService.getPatterns(this.Data.Type, val);
         if (patternList == null) { return; }
         this.PatternOptions = patternList
            .map(item => this.OnPatternParse(item));
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex); }
   }
   private OnPatternChanged(item: RelatedData<Pattern>) {
      this.Data.PatternID = null;
      if (item && item.id) {
         this.Data.PatternID = item.id;
         this.CategoryOptions = [this.OnCategoryParse(item.value.CategoryRow)];
         this.inputForm.get("Text").setValue(item.value.Text);
         this.inputForm.get("CategoryRow").setValue(this.CategoryOptions[0]);
      }
   }
   private OnPatternParse(item: Pattern): RelatedData<Pattern> {
      let badgeText: string = `${item.Count}`
      if (item.Count >= 1000000) {
         badgeText = `${Math.round(item.Count / 1000000)}m`
      }
      else if (item.Count >= 1000) {
         badgeText = `${Math.round(item.Count / 1000)}k`
      }
      return Object.assign(new RelatedData, { id: item.PatternID, description: item.Text, badgeText: badgeText, value: item });
   }



   /* ACCOUNT */
   public AccountOptions: RelatedData<Account>[] = [];
   public async OnAccountChanging(val: string) {
      try {
         let accountList = await this.accountService.getAccounts(val);
         if (accountList == null) { return; }
         accountList = accountList
            .filter(x => x.Active)
            .sort((a, b) => a.Text < b.Text ? -1 : 1);
         if (accountList.length == 0 && val == '') { this.msg.ShowInfo('ACCOUNTS_YOU_HAVE_NO_ACTIVE_ACCOUNTS_INFO'); }
         this.AccountOptions = accountList
            .map(item => this.OnAccountParse(item));
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex); }
   }
   private OnAccountChanged(item: RelatedData<Account>) {
      this.Data.AccountID = null;
      if (item && item.id) {
         this.Data.AccountID = item.id;
         const dueDateControl = this.inputForm.get("DueDate");
         dueDateControl.setValue(item.value.DueDate || this.defaultDueDate)
         dueDateControl.updateValueAndValidity();
      }
   }
   private OnAccountParse(item: Account): RelatedData<Account> {
      return Object.assign(new RelatedData, { id: item.AccountID, description: item.Text, value: item });
   }



   /* CATEGORY */
   public CategoryOptions: RelatedData<Category>[] = [];
   public async OnCategoryChanging(val: string) {
      try {
         const categoryList = await this.categoryService.getCategories(this.Data.Type, val);
         if (categoryList == null) { return; }
         if (categoryList.length == 0 && val == '') { this.msg.ShowInfo('CATEGORIES_YOU_HAVE_NO_CATEGORIES_FOR_THIS_TYPE_INFO'); }
         this.CategoryOptions = categoryList
            .map(item => this.OnCategoryParse(item));
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex); }
   }
   private OnCategoryChanged(item: RelatedData<Category>) {
      this.Data.CategoryID = null;
      if (item && item.id) {
         this.Data.CategoryID = item.id;
      }
   }
   private OnCategoryParse(item: Category): RelatedData<Category> {
      return Object.assign(new RelatedData, { id: item.CategoryID, description: item.HierarchyText, value: item });
   }



   /* PAID */
   private OnPaidChanged(paid: boolean, setValue: boolean = true) {
      const payDateControl = this.inputForm.controls['PayDate'];
      if (paid == true) {
         const today = new Date();
         let payDate = this.Data.DueDate;
         if (today < payDate) { payDate = today; }
         payDateControl.enable();
         payDateControl.setValidators([Validators.required]);
         if (setValue) { payDateControl.setValue(payDate); }
         payDateControl.markAsTouched();
      }
      else {
         payDateControl.clearValidators();
         payDateControl.markAsUntouched();
         if (setValue) { payDateControl.setValue(''); }
         payDateControl.disable();
      }
      payDateControl.updateValueAndValidity();
   }



   /* RECURRENCY: TYPES */
   public RecurrencyTypes: EnumVM<enRecurrencyType>[];

   /* RECURRENCY: ActivateChanged */
   private OnRecurrencyActivateChanged(activate: boolean) {
      this.OnRecurrencyActivateControlChanged(activate, this.inputForm.controls['RecurrencyType']);
      this.OnRecurrencyActivateControlChanged(activate, this.inputForm.controls['RecurrencyCount']);
      if (activate) {
         this.inputForm.controls['RecurrencyType'].setValue(enRecurrencyType.Monthly);
         this.inputForm.controls['RecurrencyCount'].setValue(2);
      }
   }
   private OnRecurrencyActivateControlChanged(activate: boolean, control: AbstractControl) {
      if (activate == true) {
         control.enable();
         control.setValidators([Validators.required]);
         control.markAsTouched();
      }
      else {
         control.clearValidators();
         control.markAsUntouched();
         control.disable();
      }
      control.updateValueAndValidity();
   }



   /* COMMANDS: CANCEL */
   public async OnCancelClick() {
      if (!this.inputForm.pristine) {
         if (!await this.msg.Confirm('BASE_CANCEL_CHANGES_CONFIRMATION_TEXT', 'BASE_CANCEL_CHANGES_CONFIRM', 'BASE_CANCEL_CHANGES_ABORT')) { return; }
      }
      this.service.showCurrentList();
   }

   /* COMMANDS: SAVE */
   public async OnSaveClick(editFutureRecurrencies: boolean = false) {
      try {
         if (!this.inputForm.get("RecurrencyActivate").value) { this.Data.Recurrency = null; }
         if (!await this.service.saveEntry(this.Data, editFutureRecurrencies)) { return; }
         this.service.showCurrentList();
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex); }
   }

   /* COMMANDS: REMOVE */
   public async OnRemoveClick(editFutureRecurrencies: boolean = false) {
      try {
         if (!await this.msg.Confirm('ENTRIES_REMOVE_CONFIRMATION_TEXT', 'BASE_REMOVE_COMMAND')) { return; }
         if (!await this.service.removeEntry(this.Data, editFutureRecurrencies)) { return; }
         this.service.showCurrentList();
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex); }
   }

}
