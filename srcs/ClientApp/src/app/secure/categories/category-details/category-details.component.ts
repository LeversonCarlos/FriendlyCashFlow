import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { enCategoryType, CategoriesService, Category, CategoryType } from '../categories.service';
import { MessageService } from 'src/app/shared/message/message.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RelatedData } from 'src/app/shared/related-box/related-box.models';

@Component({
   selector: 'fs-category-details',
   templateUrl: './category-details.component.html',
   styleUrls: ['./category-details.component.scss']
})
export class CategoryDetailsComponent implements OnInit {

   constructor(private service: CategoriesService, private msg: MessageService,
      private route: ActivatedRoute, private fb: FormBuilder) { }

   public Data: Category;
   public inputForm: FormGroup;
   public CategoryTypes: CategoryType[];
   public get CategoryType(): CategoryType {
      if (!this.CategoryTypes || !this.Data) { return null; }
      const categoryTypes = this.CategoryTypes.filter(x => x.Value == this.Data.Type);
      if (!categoryTypes || categoryTypes.length != 1) { return null; }
      return categoryTypes[0];
   }

   public async ngOnInit() {
      if (!await this.OnDataLoad()) { return; }
      this.service.SelectedCategoryType = this.CategoryType.Value;
      this.OnFormCreate();
   }

   private async OnDataLoad(): Promise<boolean> {

      this.CategoryTypes = await this.service.getCategoryTypes();

      const paramID: string = this.route.snapshot.params.id;
      if (paramID.startsWith('new-')) {
         const categoryType = (paramID.replace('new-', '') as any);
         this.Data = Object.assign(new Category, { Type: (categoryType == "2" ? enCategoryType.Income : enCategoryType.Expense) });
         return true;
      }

      const categoryID: number = Number(paramID);
      if (!categoryID || categoryID == 0) {
         this.msg.ShowWarning('CATEGORIES_RECORD_NOT_FOUND_WARNING');
         this.service.showList();
         return false;
      }

      this.Data = await this.service.getCategory(categoryID);
      if (!this.Data || this.Data.CategoryID != categoryID) {
         this.msg.ShowWarning('CATEGORIES_RECORD_NOT_FOUND_WARNING');
         this.service.showList();
         return false;
      }

      if (this.Data.ParentRow) {
         this.ParentOptions = [this.OnParentParse(this.Data.ParentRow)];
      }

      return true;
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Text: [this.Data.Text, Validators.required],
         ParentRow: [this.ParentOptions && this.ParentOptions.length ? this.ParentOptions[0] : null]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         this.Data.ParentID = null;
         if (values.ParentRow && values.ParentRow.id) {
            this.Data.ParentID = values.ParentRow.id;
         }
      });
   }

   public ParentOptions: RelatedData<Category>[] = [];
   public async OnParentChanging(val: string) {
      const categoryList = await this.service.getCategories(this.Data.Type, val);
      if (categoryList == null) { return; }
      this.ParentOptions = categoryList
         .map(item => this.OnParentParse(item));
   }
   private OnParentParse(item: Category): RelatedData<Category> {
      return Object.assign(new RelatedData, { id: item.CategoryID, description: item.HierarchyText, value: item });
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine) {
         if (!await this.msg.Confirm('BASE_CANCEL_CHANGES_CONFIRMATION_TEXT', 'BASE_CANCEL_CHANGES_CONFIRM', 'BASE_CANCEL_CHANGES_ABORT')) { return; }
      }
      this.service.showList();
   }

   public async OnSaveClick() {
      if (!await this.service.saveCategory(this.Data)) { return; }
      this.service.showList();
   }

   public async OnRemoveClick() {
      if (!await this.msg.Confirm('CATEGORIES_REMOVE_CONFIRMATION_TEXT', 'BASE_REMOVE_COMMAND')) { return; }
      if (!await this.service.removeCategory(this.Data)) { return; }
      this.service.showList();
   }

}
