import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { enCategoryType, CategoriesService, Category } from '../categories.service';
import { MessageService } from 'src/app/shared/message/message.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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

   public async ngOnInit() {
      if (!await this.OnDataLoad()) { return; }
      this.OnFormCreate();
   }

   private async OnDataLoad(): Promise<boolean> {

      const paramID: string = this.route.snapshot.params.id;
      if (paramID.startsWith('new-')) {
         const categoryType: enCategoryType = (paramID.replace('new-', '') as any);
         this.Data = Object.assign(new Category, { Type: categoryType });
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

      return true;
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Text: [this.Data.Text, Validators.required],
         ParentID: [this.Data.ParentID]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         this.Data.ParentID = values.ParentID;
      });
   }

}
