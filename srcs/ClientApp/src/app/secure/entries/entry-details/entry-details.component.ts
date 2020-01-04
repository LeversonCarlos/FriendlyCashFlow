import { Component, OnInit } from '@angular/core';
import { Entry, EntriesService } from '../entries.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'src/app/shared/message/message.service';
import { ActivatedRoute } from '@angular/router';

@Component({
   selector: 'fs-entry-details',
   templateUrl: './entry-details.component.html',
   styleUrls: ['./entry-details.component.scss']
})
export class EntryDetailsComponent implements OnInit {

   constructor(private service: EntriesService, private msg: MessageService,
      private route: ActivatedRoute, private fb: FormBuilder) { }

   public Data: Entry;
   public inputForm: FormGroup;

   public async ngOnInit() {
      if (!await this.OnDataLoad()) { return; }
      this.OnFormCreate();
   }

   private async OnDataLoad(): Promise<boolean> {
      try {

         const paramID: string = this.route.snapshot.params.id;
         if (paramID == 'new') { this.Data = Object.assign(new Entry, { Active: true }); return true; }

         const entryID: number = Number(paramID);
         if (!entryID || entryID == 0) {
            this.msg.ShowWarning('ENTRIES_RECORD_NOT_FOUND_WARNING');
            this.service.showFlow();
            return false;
         }

         this.Data = await this.service.getEntry(entryID);
         if (!this.Data || this.Data.EntryID != entryID) {
            this.msg.ShowWarning('ENTRIES_RECORD_NOT_FOUND_WARNING');
            this.service.showFlow();
            return false;
         }

         return true;
      }
      catch (ex) { console.error(ex) }
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Text: [this.Data.Text, Validators.required],
         EntryValue: [this.Data.EntryValue, Validators.required],
         DueDate: [this.Data.DueDate, Validators.required],
         Paid: [this.Data.Paid],
         PayDate: [this.Data.EntryValue]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         this.Data.EntryValue = values.EntryValue;
         this.Data.DueDate = values.DueDate;
         this.Data.Paid = values.Paid || false;
         this.Data.PayDate = values.PayDate;
      });
      this.inputForm.controls['Paid'].valueChanges.subscribe((paid) => {
         console.log('Paid', paid)
      });
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine) {
         if (!await this.msg.Confirm('BASE_CANCEL_CHANGES_CONFIRMATION_TEXT', 'BASE_CANCEL_CHANGES_CONFIRM', 'BASE_CANCEL_CHANGES_ABORT')) { return; }
      }
      this.service.showFlow();
   }

   public async OnSaveClick() {
      // if (!await this.service.saveAccount(this.Data)) { return; }
      // this.service.showList();
   }

   public async OnRemoveClick() {
      if (!await this.msg.Confirm('ENTRIES_REMOVE_CONFIRMATION_TEXT', 'BASE_REMOVE_COMMAND')) { return; }
      // if (!await this.service.removeAccount(this.Data)) { return; }
      // this.service.showList();
   }

}
