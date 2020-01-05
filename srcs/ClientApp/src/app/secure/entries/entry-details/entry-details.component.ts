import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'src/app/shared/message/message.service';
import { ActivatedRoute } from '@angular/router';
import { Entry } from '../entries.viewmodels';
import { RelatedData } from 'src/app/shared/related-box/related-box.models';
import { Category, CategoriesService } from '../../categories/categories.service';

@Component({
   selector: 'fs-entry-details',
   templateUrl: './entry-details.component.html',
   styleUrls: ['./entry-details.component.scss']
})
export class EntryDetailsComponent implements OnInit {

   constructor(private service: EntriesService, private msg: MessageService,
      private categoryService: CategoriesService,
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
         if (paramID == 'new') { this.Data = Object.assign(new Entry, { Active: true }); return true; }

         const entryID: number = Number(paramID);
         if (!entryID || entryID == 0) {
            this.msg.ShowWarning('ENTRIES_RECORD_NOT_FOUND_WARNING');
            this.service.showFlow();
            return false;
         }

         this.Data = await this.service.getEntry(entryID);
         if (!this.Data || this.Data.EntryID != entryID) {
            this.msg.ShowWarning('ENTRIES_RECORD_NOT_FOUND_WARNING');
            this.service.showFlow();
            return false;
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
         EntryValue: [this.Data.EntryValue, Validators.required],
         DueDate: [this.Data.DueDate, Validators.required],
         CategoryRow: [this.CategoryOptions && this.CategoryOptions.length ? this.CategoryOptions[0] : null],
         Paid: [this.Data.Paid],
         PayDate: [this.Data.EntryValue]
      });
      this.inputForm.get("CategoryRow").valueChanges.subscribe(value => {
         this.Data.CategoryID = null;
         if (value && value.id) {
            this.Data.CategoryID = value.id;
         }
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         this.Data.EntryValue = values.EntryValue;
         this.Data.DueDate = values.DueDate;
         this.Data.Paid = values.Paid || false;
         this.Data.PayDate = values.PayDate;
      });
      this.inputForm.controls['Paid'].valueChanges.subscribe((paid) => {
         console.log('Paid', paid)
      });
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

   public async OnCancelClick() {
      if (!this.inputForm.pristine) {
         if (!await this.msg.Confirm('BASE_CANCEL_CHANGES_CONFIRMATION_TEXT', 'BASE_CANCEL_CHANGES_CONFIRM', 'BASE_CANCEL_CHANGES_ABORT')) { return; }
      }
      this.service.showFlow();
   }

   public async OnSaveClick() {
      // if (!await this.service.saveAccount(this.Data)) { return; }
      // this.service.showList();
   }

   public async OnRemoveClick() {
      if (!await this.msg.Confirm('ENTRIES_REMOVE_CONFIRMATION_TEXT', 'BASE_REMOVE_COMMAND')) { return; }
      // if (!await this.service.removeAccount(this.Data)) { return; }
      // this.service.showList();
   }

}
