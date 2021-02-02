import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService, RelatedData } from '@elesse/shared';
import { CategoryEntity, enCategoryType } from '../../model/categories.model';
import { CategoriesData } from '../../data/categories.data';

@Component({
   selector: 'categories-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private categoriesData: CategoriesData, private busy: BusyService, private msg: MessageService,
      private fb: FormBuilder, private activatedRoute: ActivatedRoute, private router: Router) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }
   public Type: enCategoryType;
   public get IsIncome(): boolean { return this.Type == enCategoryType.Income; }
   public get IsExpense(): boolean { return this.Type == enCategoryType.Expense; }

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;

      const data = await this.categoriesData.LoadCategory(paramID);
      if (!data)
         this.router.navigate(["/categories/list"])

      this.Type = data.Type;

      this.ParentOptions = this.categoriesData.GetCategories(data.Type)
         .filter(entity => entity.CategoryID != data.CategoryID)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.CategoryID,
            description: entity.HierarchyText,
            value: entity
         }));

      if (data.ParentID)
         this.ParentFiltered = this.ParentOptions
            .filter(entity => entity.value.CategoryID == data.ParentID)

      this.OnFormCreate(data);
   }

   private OnFormCreate(data: CategoryEntity) {
      this.inputForm = this.fb.group({
         CategoryID: [data.CategoryID],
         Text: [data.Text, Validators.required],
         Type: [data.Type],
         ParentID: [data?.ParentID],
         ParentRow: [this.ParentFiltered?.length == 1 ? this.ParentFiltered[0] : null]
      });
      this.inputForm.controls['ParentRow'].valueChanges.subscribe((parentRow: RelatedData<CategoryEntity>) => {
         this.inputForm.controls['ParentID'].setValue(parentRow?.value?.CategoryID ?? null);
      });
   }

   public ParentOptions: RelatedData<CategoryEntity>[] = [];
   public ParentFiltered: RelatedData<CategoryEntity>[] = [];
   public async OnParentChanging(val: string) {
      this.ParentFiltered = this.ParentOptions
         .filter(entity => entity.value.HierarchyText.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine)
         if (!await this.msg.Confirm('shared.ROLLBACK_TEXT', 'shared.ROLLBACK_CONFIRM_COMMAND', 'shared.ROLLBACK_ABORT_COMMAND'))
            return;
      this.router.navigate(["/categories/list"])
   }

   public async OnSaveClick() {
      if (!this.inputForm.valid)
         return;
      const data: CategoryEntity = Object.assign(new CategoryEntity, this.inputForm.value);
      if (!await this.categoriesData.SaveCategory(data))
         return;
      this.router.navigate(["/categories/list"])
   }

}
