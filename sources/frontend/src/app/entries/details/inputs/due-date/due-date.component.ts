import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { EntryEntity } from '@elesse/entries';
import { ControlNames } from '../../details.control-names';

@Component({
   selector: 'entries-details-due-date',
   templateUrl: './due-date.component.html',
   styleUrls: ['./due-date.component.scss']
})
export class DueDateComponent implements OnInit {

   constructor() { }

   @Input() data: EntryEntity;
   @Input() form: FormGroup;
   public formControlName: string = ControlNames.DueDate;

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
      this.form.addControl(ControlNames.DueDate, new FormControl(this.data.DueDate ?? null, Validators.required));
      this.form.get(ControlNames.DueDate).valueChanges.subscribe((val: string) => {
         let dueDate = new Date(val);
         dueDate = new Date(dueDate.getFullYear(), dueDate.getMonth(), dueDate.getDate(), 12);
         this.data.DueDate = dueDate;
      });
   }

}
