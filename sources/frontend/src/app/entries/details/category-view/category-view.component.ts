import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CategoriesData, CategoryEntity } from '@elesse/categories';
import { RelatedData } from '@elesse/shared';
import { EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-details-category',
   templateUrl: './category-view.component.html',
   styleUrls: ['./category-view.component.scss']
})
export class CategoryViewComponent implements OnInit {

   constructor(private categoriesData: CategoriesData) { }

   ngOnInit(): void {
      this.OnDataInit();
      this.OnFormInit();
   }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public CategoryOptions: RelatedData<CategoryEntity>[] = [];
   public CategoryFiltered: RelatedData<CategoryEntity>[] = [];

   private OnDataInit() {
      this.CategoryOptions = this.categoriesData.GetCategories(this.data.Pattern.Type)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.CategoryID,
            description: entity.HierarchyText,
            value: entity
         }));
      if (this.data.Pattern.CategoryID)
         this.CategoryFiltered = this.CategoryOptions
            .filter(entity => entity.value.CategoryID == this.data.Pattern.CategoryID)
   }

   private OnFormInit() {
      const formSection = this.form.get("Pattern") as FormGroup;
      formSection.addControl("CategoryID", new FormControl(this.data.Pattern.CategoryID));
      formSection.addControl("CategoryRow", new FormControl(this.CategoryFiltered?.length == 1 ? this.CategoryFiltered[0] : null, Validators.required));
      this.form.get("Pattern.CategoryRow").valueChanges.subscribe((row: RelatedData<CategoryEntity>) => {
         this.form.get("Pattern.CategoryID").setValue(row?.value?.CategoryID ?? null);
      });
      this.form.get("Pattern.CategoryID").valueChanges.subscribe((categoryID: string) => {
         this.CategoryFiltered = this.CategoryOptions
            .filter(entity => entity.value.CategoryID == categoryID);
         const categoryRow = this.CategoryFiltered?.length == 1 ? this.CategoryFiltered[0] : null;
         if (this.form.get("Pattern.CategoryRow").value != categoryRow)
            this.form.get("Pattern.CategoryRow").setValue(categoryRow);
      });
   }

   public async OnCategoryChanging(val: string) {
      this.CategoryFiltered = this.CategoryOptions
         .filter(entity => entity.value.HierarchyText.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

}
