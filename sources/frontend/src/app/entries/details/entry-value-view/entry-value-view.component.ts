import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-details-entry-value',
   templateUrl: './entry-value-view.component.html',
   styleUrls: ['./entry-value-view.component.scss']
})
export class EntryValueViewComponent implements OnInit {

   constructor() { }

   ngOnInit(): void {
      this.OnDataInit();
      this.OnFormInit();
   }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;

   private OnDataInit() {
      if (!this.data)
         return;
   }

   private OnFormInit() {
      if (!this.form)
         return;
      this.form.addControl("EntryValue", new FormControl(this.data?.EntryValue ?? null, [Validators.required, Validators.min(0.01)]));
      this.form.get("EntryValue").valueChanges.subscribe((val: any) => {
         this.data.EntryValue = val;
      });
   }

}
