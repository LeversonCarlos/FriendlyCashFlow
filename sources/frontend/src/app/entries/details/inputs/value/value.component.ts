import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EntryEntity } from '@elesse/entries';

@Component({
   selector: 'entries-details-value',
   templateUrl: './value.component.html',
   styleUrls: ['./value.component.scss']
})
export class ValueComponent implements OnInit {

   constructor() { }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public FormControlName: string = "Value";

   ngOnInit(): void {
      this.OnDataInit();
      this.OnFormInit();
   }

   private OnDataInit() {
      if (!this.data)
         return;
   }

   private OnFormInit() {
      if (!this.form || !this.data)
         return;
      this.form.addControl(this.FormControlName, new FormControl(this.data.Value ?? null, [Validators.required, Validators.min(0.01)]));
      this.form.get(this.FormControlName).valueChanges.subscribe((val: any) => {
         this.data.Value = val;
      });
   }

}
