import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { PatternEntity, PatternsData } from '@elesse/patterns';
import { RelatedData } from '@elesse/shared';
import { EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-details-pattern',
   templateUrl: './pattern-view.component.html',
   styleUrls: ['./pattern-view.component.scss']
})
export class PatternViewComponent implements OnInit {

   constructor(private patternsData: PatternsData) { }

   public ngOnInit(): void {
      if (!this.data)
         return;
      this.patternsData.OnObservableFirstPush(this.data.Pattern.Type, entities => {
         this.OnDataInit();
         this.OnFormInit();
      });
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
      if (!this.form)
         return;
      this.formSection = this.form.get("Pattern") as FormGroup;
      this.formSection.addControl("PatternID", new FormControl(this.data?.Pattern?.PatternID ?? null));
      this.formSection.addControl("PatternRow", new FormControl(this.GetFirstPattern()));
      this.form.get("Pattern.PatternRow").valueChanges.subscribe((row: RelatedData<PatternEntity>) => {
         this.data.Pattern.PatternID = row?.value?.PatternID ?? null;
         if (row?.value?.Text)
            this.OnTextChanging(row.value.Text);
         if (row?.value?.CategoryID)
            this.form.get("Pattern.CategoryID").setValue(row.value.CategoryID);
      });
   }

   public async OnPatternChanging(val: string) {
      this.data.Pattern.Text = val;
      this.PatternFiltered = this.PatternOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   private GetFirstPattern(): RelatedData<PatternEntity> {
      return this.PatternFiltered?.length == 1 ? this.PatternFiltered[0] : null
   }

}
