import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { PatternEntity, PatternsData } from '@elesse/patterns';
import { RelatedData } from '@elesse/shared';
import { EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-details-pattern',
   templateUrl: './pattern-view.component.html',
   styleUrls: ['./pattern-view.component.scss']
})
export class PatternViewComponent implements OnInit {

   constructor(private patternsData: PatternsData,
      private fb: FormBuilder) { }

   public ngOnInit(): void {
      this.OnDataInit();
      this.OnFormInit();
   }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public PatternOptions: RelatedData<PatternEntity>[] = [];
   public PatternFiltered: RelatedData<PatternEntity>[] = [];

   private OnDataInit() {
      if (!this.data)
         return;
      this.PatternOptions = this.patternsData.GetPatterns(this.data.Pattern.Type)
         .map(entity => Object.assign(new RelatedData, {
            id: entity.PatternID,
            description: entity.Text,
            value: entity
         }));
      if (this.data.Pattern.PatternID)
         this.PatternFiltered = this.PatternOptions
            .filter(entity => entity.value.PatternID == this.data.Pattern.PatternID)
   }

   private OnFormInit() {
      if (!this.form)
         return;
      const formSection = this.form.get("Pattern") as FormGroup;
      formSection.addControl("PatternID", new FormControl(this.data?.Pattern?.PatternID ?? null));
      formSection.addControl("PatternRow", new FormControl(this.PatternFiltered?.length == 1 ? this.PatternFiltered[0] : null, Validators.required));
      this.form.get("Pattern.PatternRow").valueChanges.subscribe((row: RelatedData<PatternEntity>) => {
         this.form.get("Pattern.PatternID").setValue(row?.value?.PatternID ?? null);
         this.form.get("Pattern.Text").setValue(row?.value?.Text ?? null);
         this.form.get("Pattern.CategoryID").setValue(row?.value?.CategoryID ?? null);
      });
   }

   public async OnPatternChanging(val: string) {
      this.PatternFiltered = this.PatternOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

}
