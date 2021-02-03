import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EntryEntity } from '../../model/entries.model';

@Component({
   selector: 'entries-details-due-date',
   templateUrl: './due-date-view.component.html',
   styleUrls: ['./due-date-view.component.scss']
})
export class DueDateViewComponent implements OnInit {

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
      this.form.addControl("DueDate", new FormControl(this.data?.DueDate ?? null, Validators.required));
      this.form.get("DueDate").valueChanges.subscribe((val: string) => {
         let dueDate = new Date(val);
         dueDate = new Date(dueDate.getFullYear(), dueDate.getMonth(), dueDate.getDate(), 12);
         this.form.get("DueDate").setValue(dueDate);
      });
   }

}
