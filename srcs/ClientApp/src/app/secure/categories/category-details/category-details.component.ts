import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { enCategoryType, CategoriesService, Category } from '../categories.service';
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
   private categoryType: enCategoryType;

   public async ngOnInit() {
      if (!await this.OnDataLoad()) { return; }
      this.OnFormCreate();
   }

   private async OnDataLoad(): Promise<boolean> {

      const paramID: string = this.route.snapshot.params.id;
      if (paramID.startsWith('new-')) {
         this.categoryType = (paramID.replace('new-', '') as any);
         this.Data = Object.assign(new Category, { Type: this.categoryType });
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
         Type: [this.categoryType],
         ParentID: [this.Data.ParentID],
         ParentRow: [this.ParentOptions && this.ParentOptions.length ? this.ParentOptions[0] : null]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         values.ParentID = null;
         if (values.ParentRow && values.ParentRow.id) {
            values.ParentID = values.ParentRow.id;
         }
         this.Data.ParentID = values.ParentID;
      });
   }

   public ParentOptions: RelatedData<Category>[] = [];
   public async OnParentChanging(val: string) {
      const categoryList = await this.service.getCategories(this.categoryType, val);
      this.ParentOptions = categoryList
         .map(item => this.OnParentParse(item));
   }
   private OnParentParse(item: Category) {
      return Object.assign(new RelatedData, { id: item.CategoryID, description: item.Text, value: item });
   }

}
