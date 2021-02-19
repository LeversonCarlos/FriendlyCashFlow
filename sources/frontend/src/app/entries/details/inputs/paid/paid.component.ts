import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { EntryEntity } from 'src/app/entries/model/entries.model';

@Component({
   selector: 'entries-details-paid',
   templateUrl: './paid.component.html',
   styleUrls: ['./paid.component.scss']
})
export class PaidComponent implements OnInit {

   constructor() { }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public FormControlName: string = "Paid";

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
      this.form.addControl(this.FormControlName, new FormControl(this.data.Paid ?? false));
   }

}
