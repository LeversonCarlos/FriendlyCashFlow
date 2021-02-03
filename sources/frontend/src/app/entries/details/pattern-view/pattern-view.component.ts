import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
   public formSection: FormGroup;
   public PatternOptions: RelatedData<PatternEntity>[] = [];
   public PatternFiltered: RelatedData<PatternEntity>[] = [];

   private OnDataInit() {
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
      this.formSection = this.fb.group({
         PatternID: [this.data.Pattern.PatternID],
         PatternRow: [this.PatternFiltered?.length == 1 ? this.PatternFiltered[0] : null, Validators.required],
      });
      this.formSection.get("PatternRow").valueChanges.subscribe((row: RelatedData<PatternEntity>) => {
         this.formSection.get("PatternID").setValue(row?.value?.PatternID ?? null);
         this.formSection.get("Text").setValue(row?.value?.Text ?? null);
         this.formSection.get("CategoryID").setValue(row?.value?.CategoryID ?? null);
      });
      this.form.registerControl("Patterns", this.formSection);
   }

   public async OnPatternChanging(val: string) {
      this.PatternFiltered = this.PatternOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

}
