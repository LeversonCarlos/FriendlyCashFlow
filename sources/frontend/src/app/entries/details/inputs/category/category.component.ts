import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CategoriesData, CategoryEntity } from '@elesse/categories';
import { EntryEntity } from '@elesse/entries';
import { RelatedData } from '@elesse/shared';

@Component({
   selector: 'entries-details-category',
   templateUrl: './category.component.html',
   styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit {

   constructor(private categoriesData: CategoriesData) { }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public formControlName: string = CategoryControlNames.CategoryRow;
   public CategoryOptions: RelatedData<CategoryEntity>[] = [];
   public CategoryFiltered: RelatedData<CategoryEntity>[] = [];
   public FormSectionName: string = 'Pattern';

   ngOnInit(): void {
      if (!this.data)
         return;
      this.categoriesData.OnObservableFirstPush(this.data.Pattern.Type, () => {
         this.OnDataInit();
         this.OnFormInit();
      });
   }

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
      if (!this.form || !this.data)
         return;
      const formSection = this.form.get(this.FormSectionName) as FormGroup;
      formSection.addControl(CategoryControlNames.CategoryID, new FormControl(this.data.Pattern?.CategoryID ?? null));
      formSection.addControl(CategoryControlNames.CategoryRow, new FormControl(this.GetFirstCategory(), Validators.required));
      formSection.get(CategoryControlNames.CategoryRow).valueChanges.subscribe((row: RelatedData<CategoryEntity>) => {
         this.data.Pattern.CategoryID = row?.value?.CategoryID ?? null;
      });
      formSection.get(CategoryControlNames.CategoryID).valueChanges.subscribe((categoryID: string) => {
         this.CategoryFiltered = this.CategoryOptions
            .filter(entity => entity.value.CategoryID == categoryID);
         const categoryRow = this.GetFirstCategory();
         if (formSection.get(CategoryControlNames.CategoryRow).value != categoryRow)
            formSection.get(CategoryControlNames.CategoryRow).setValue(categoryRow);
      });
   }

   public async OnCategoryChanging(val: string) {
      this.CategoryFiltered = this.CategoryOptions
         .filter(entity => entity.value.HierarchyText.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   private GetFirstCategory(): RelatedData<CategoryEntity> {
      return this.CategoryFiltered?.length == 1 ? this.CategoryFiltered[0] : null;
   }

}

export const CategoryControlNames = {
   CategoryID: 'CategoryID',
   CategoryRow: 'CategoryRow'
}
