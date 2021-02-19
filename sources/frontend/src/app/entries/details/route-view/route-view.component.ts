import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService } from '@elesse/shared';
import { enCategoryType } from '@elesse/categories';
import { EntryEntity } from '../../model/entries.model';
import { EntriesData } from '../../data/entries.data';

@Component({
   selector: 'entries-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private entriesData: EntriesData,
      private msg: MessageService, private busy: BusyService,
      private activatedRoute: ActivatedRoute, private router: Router, private fb: FormBuilder) { }

   public data: EntryEntity;
   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   private get Type(): enCategoryType { return this.data?.Pattern?.Type; }
   public get IsIncome(): boolean { return this.Type == enCategoryType.Income; }
   public get IsExpense(): boolean { return this.Type == enCategoryType.Expense; }

   public async ngOnInit(): Promise<void> {
      this.data = await this.entriesData.LoadEntry(this.activatedRoute.snapshot);
      if (!this.data) {
         this.router.navigate(["/transactions/list"]);
         return;
      }
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Pattern: this.fb.group({
            Type: [this.data.Pattern.Type],
            Text: [this.data.Pattern.Text, Validators.required]
         }),
      });
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine)
         if (!await this.msg.Confirm('shared.ROLLBACK_TEXT', 'shared.ROLLBACK_CONFIRM_COMMAND', 'shared.ROLLBACK_ABORT_COMMAND'))
            return;
      this.router.navigate(["/transactions/list"])
   }

   public async OnSaveClick() {
      if (!this.inputForm.valid)
         return;
      if (!await this.entriesData.SaveEntry(this.data))
         return;
      this.router.navigate(["/transactions/list"])
   }

}
