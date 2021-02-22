import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { EntryEntity } from '@elesse/entries';
import { PatternEntity, PatternsData } from '@elesse/patterns';
import { RelatedData } from '@elesse/shared';
import { ControlNames } from '../../details.control-names';

@Component({
   selector: 'entries-details-pattern',
   templateUrl: './pattern.component.html',
   styleUrls: ['./pattern.component.scss']
})
export class PatternComponent implements OnInit {

   constructor(private patternsData: PatternsData) { }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public formControlName: string = ControlNames.PatternRow;
   public formSection: FormGroup;
   public PatternOptions: RelatedData<PatternEntity>[] = [];
   public PatternFiltered: RelatedData<PatternEntity>[] = [];

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
         .map(entity => this.ConvertPatternIntoGetRelatedData(entity));
      if (this.data.Pattern.PatternID)
         this.PatternFiltered = this.PatternOptions
            .filter(entity => entity.value.PatternID == this.data.Pattern.PatternID)
   }

   private OnFormInit() {
      if (!this.form)
         return;
      this.formSection = this.form.get(ControlNames.Pattern) as FormGroup;
      this.formSection.addControl(ControlNames.PatternID, new FormControl(this.data?.Pattern?.PatternID ?? null));
      this.formSection.addControl(ControlNames.PatternRow, new FormControl(this.GetFirstPattern()));
      this.formSection.get(ControlNames.PatternRow).valueChanges.subscribe((row: RelatedData<PatternEntity>) => {
         this.data.Pattern.PatternID = row?.value?.PatternID ?? null;
         if (row?.value?.Text)
            this.OnTextChanging(row.value.Text);
         if (row?.value?.CategoryID)
            this.formSection.get(ControlNames.CategoryID).setValue(row.value.CategoryID);
      });
   }

   public async OnPatternChanging(val: string) {
      this.OnTextChanging(val);
      this.PatternFiltered = this.PatternOptions
         .filter(entity => entity.value.Text.search(new RegExp(val, 'i')) != -1)
         .sort((a, b) => a.description > b.description ? 1 : -1)
   }

   private OnTextChanging(val: string) {
      this.formSection.get(ControlNames.Text).setValue(val);
      this.data.Pattern.Text = val;
   }

   private ConvertPatternIntoGetRelatedData(entity: PatternEntity): RelatedData<PatternEntity> {
      return Object.assign(new RelatedData, {
         id: entity.PatternID,
         description: entity.Text,
         badgeText: this.GetPatternBadge(entity),
         value: entity
      });
   }

   private GetPatternBadge(entity: PatternEntity): string {
      if (entity.RowsCount >= 1000000)
         return `${Math.round(entity.RowsCount / 1000000)}m`
      else if (entity.RowsCount >= 1000)
         return `${Math.round(entity.RowsCount / 1000)}k`
      else
         return `${entity.RowsCount}`
   }

   private GetFirstPattern(): RelatedData<PatternEntity> {
      return this.PatternFiltered?.length == 1 ? this.PatternFiltered[0] : null
   }

}
