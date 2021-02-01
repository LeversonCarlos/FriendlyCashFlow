import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService, RelatedData } from '@elesse/shared';
import { CategoriesData, CategoryEntity } from '@elesse/categories';
import { EntryEntity } from '../../model/entries.model';
import { EntriesData } from '../../data/entries.data';

@Component({
   selector: 'entries-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private entriesData: EntriesData, private categoriesData: CategoriesData,
      private msg: MessageService, private busy: BusyService,
      private activatedRoute: ActivatedRoute, private router: Router, private fb: FormBuilder) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }
   public TypeDescription: string;

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;

      const data = await this.entriesData.LoadEntry(paramID);
      if (!data)
         this.router.navigate(["/entries/list"])

      this.TypeDescription = (await this.categoriesData.GetCategoryTypes())
         .filter(cat => cat.Value == data.Pattern.Type)
         .map(cat => cat.Text)
         .reduce((a, b) => b, '');

      this.CategoryOptions = this.categoriesData.GetCategories(data.Pattern.Type)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.CategoryID,
            description: entity.HierarchyText,
            value: entity
         }));

      if (data.Pattern.CategoryID)
         this.CategoryFiltered = this.CategoryOptions
            .filter(entity => entity.value.CategoryID == data.Pattern.CategoryID)

      this.OnFormCreate(data);
   }

   private OnFormCreate(data: EntryEntity) {
      this.inputForm = this.fb.group({
         PatternID: [data.Pattern.PatternID],
         Pattern: this.fb.group({
            Type: [data.Pattern.Type],
            CategoryID: [data.Pattern.CategoryID],
            CategoryRow: [this.CategoryFiltered?.length == 1 ? this.CategoryFiltered[0] : null, Validators.required],
            Text: [data.Pattern.Text, Validators.required]
         }),
         AccountID: [data.AccountID, Validators.required],
         DueDate: [data.DueDate, Validators.required],
         EntryValue: [data.EntryValue, [Validators.required, Validators.min(0.01)]],
         Paid: [data.Paid],
         PayDate: [data.PayDate],
      });
      this.inputForm.get("Pattern").get("CategoryRow").valueChanges.subscribe((row: RelatedData<CategoryEntity>) => {
         this.inputForm.get("Pattern").get("CategoryID").setValue(row?.value?.CategoryID ?? null);
      });
   }

   public CategoryOptions: RelatedData<CategoryEntity>[] = [];
   public CategoryFiltered: RelatedData<CategoryEntity>[] = [];
   public async OnCategoryChanging(val: string) {
      this.CategoryFiltered = this.CategoryOptions
         .filter(entity => entity.value.HierarchyText.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine)
         if (!await this.msg.Confirm('shared.ROLLBACK_TEXT', 'shared.ROLLBACK_CONFIRM_COMMAND', 'shared.ROLLBACK_ABORT_COMMAND'))
            return;
      this.router.navigate(["/entries/list"])
   }

   public async OnSaveClick() {
      if (!this.inputForm.valid)
         return;
      const data: EntryEntity = Object.assign(new EntryEntity, this.inputForm.value);
      if (!await this.entriesData.SaveEntry(data))
         return;
      this.router.navigate(["/entries/list"])
   }

}
