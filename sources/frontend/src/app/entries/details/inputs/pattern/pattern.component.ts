import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { EntryEntity } from '@elesse/entries';
import { PatternEntity, PatternsData } from '@elesse/patterns';
import { RelatedData } from '@elesse/shared';

@Component({
   selector: 'entries-details-pattern',
   templateUrl: './pattern.component.html',
   styleUrls: ['./pattern.component.scss']
})
export class PatternComponent implements OnInit {

   constructor(private patternsData: PatternsData) { }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public formSection: FormGroup;
   public PatternOptions: RelatedData<PatternEntity>[] = [];
   public PatternFiltered: RelatedData<PatternEntity>[] = [];
   private FormControlID: string = "PatternID";
   public FormControlName: string = `${this.FormControlID}Row`;
   public FormSectionName: string = 'Pattern';

   ngOnInit(): void {
      if (!this.data)
         return;
      this.patternsData.OnObservableFirstPush(this.data.Pattern.Type, () => {
         this.OnDataInit();
         this.OnFormInit();
      });
   }

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
      this.formSection = this.form.get(this.FormSectionName) as FormGroup;
      this.formSection.addControl(this.FormControlID, new FormControl(this.data?.Pattern?.PatternID ?? null));
      this.formSection.addControl(this.FormControlName, new FormControl(this.GetFirstPattern()));
      this.formSection.get(this.FormControlName).valueChanges.subscribe((row: RelatedData<PatternEntity>) => {
         this.data.Pattern.PatternID = row?.value?.PatternID ?? null;
         if (row?.value?.Text)
            this.OnTextChanging(row.value.Text);
         if (row?.value?.CategoryID)
            this.formSection.get("CategoryID").setValue(row.value.CategoryID);
      });
   }

   public async OnPatternChanging(val: string) {
      this.OnTextChanging(val);
      this.PatternFiltered = this.PatternOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   private OnTextChanging(val: string) {
      this.formSection.get("Text").setValue(val);
      this.data.Pattern.Text = val;
   }

   private GetFirstPattern(): RelatedData<PatternEntity> {
      return this.PatternFiltered?.length == 1 ? this.PatternFiltered[0] : null
   }

}
