import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CategoriesData, CategoryEntity } from '@elesse/categories';
import { EntryEntity } from '@elesse/entries';
import { RelatedData } from '@elesse/shared';
import { ControlNames } from '../../details.control-names';

@Component({
   selector: 'entries-details-category',
   templateUrl: './category.component.html',
   styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit {

   constructor(private categoriesData: CategoriesData) { }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public formControlName: string = ControlNames.CategoryRow;
   public CategoryOptions: RelatedData<CategoryEntity>[] = [];
   public CategoryFiltered: RelatedData<CategoryEntity>[] = [];

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
      const formSection = this.form.get(ControlNames.Pattern) as FormGroup;
      formSection.addControl(ControlNames.CategoryID, new FormControl(this.data.Pattern?.CategoryID ?? null));
      formSection.addControl(ControlNames.CategoryRow, new FormControl(this.GetFirstCategory(), Validators.required));
      formSection.get(ControlNames.CategoryRow).valueChanges.subscribe((row: RelatedData<CategoryEntity>) => {
         this.data.Pattern.CategoryID = row?.value?.CategoryID ?? null;
      });
      formSection.get(ControlNames.CategoryID).valueChanges.subscribe((categoryID: string) => {
         this.CategoryFiltered = this.CategoryOptions
            .filter(entity => entity.value.CategoryID == categoryID);
         const categoryRow = this.GetFirstCategory();
         if (formSection.get(ControlNames.CategoryRow).value != categoryRow)
            formSection.get(ControlNames.CategoryRow).setValue(categoryRow);
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
