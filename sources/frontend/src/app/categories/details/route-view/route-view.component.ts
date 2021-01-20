import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService, RelatedData } from '@elesse/shared';
import { CategoryEntity } from '../../categories.data';
import { CategoriesService } from '../../categories.service';

@Component({
   selector: 'categories-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private service: CategoriesService, private busy: BusyService, private msg: MessageService,
      private fb: FormBuilder, private activatedRoute: ActivatedRoute, private router: Router) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;

      const data = await this.service.LoadCategory(paramID);
      if (!data)
         this.router.navigate(["/categories/list"])

      this.ParentOptions = this.service.GetCategories(data.Type)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.CategoryID,
            description: entity.Text,
            value: entity
         }));

      if (data.ParentID)
         this.ParentFiltered = this.ParentOptions
            .filter(entity => entity.value.CategoryID == data.ParentID)

      this.OnFormCreate(data);
   }

   private OnFormCreate(data: CategoryEntity) {
      this.inputForm = this.fb.group({
         CategoryID: [data?.CategoryID],
         Text: [data.Text, Validators.required],
         Type: new FormControl({ value: data.Type, disabled: true }),
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
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
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
      if (!await this.service.SaveCategory(data))
         return;
      this.router.navigate(["/categories/list"])
   }

}
