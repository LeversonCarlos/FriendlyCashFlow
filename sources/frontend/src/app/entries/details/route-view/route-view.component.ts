import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService } from '@elesse/shared';
import { CategoriesService } from 'src/app/categories/categories.service';
import { EntryEntity } from '../../model/entries.model';
import { EntriesService } from '../../entries.service';

@Component({
   selector: 'entries-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private service: EntriesService, private categoryService: CategoriesService,
      private msg: MessageService, private busy: BusyService,
      private activatedRoute: ActivatedRoute, private router: Router, private fb: FormBuilder) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }
   public TypeDescription: string;

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;

      const data = await this.service.LoadEntry(paramID);
      if (!data)
         this.router.navigate(["/entries/list"])

      this.TypeDescription = (await this.categoryService.GetAccountTypes())
         .filter(cat => cat.Value == data.Pattern.Type)
         .map(cat => cat.Text)
         .reduce((a, b) => b, '');

      this.OnFormCreate(data);
   }

   private OnFormCreate(data: EntryEntity) {
      this.inputForm = this.fb.group({
         PatternID: [data.Pattern.CategoryID],
         Text: [data.Pattern.Text, Validators.required],
         Type: [data.Pattern.Type],
         AccountID: [data.AccountID, Validators.required],
         DueDate: [data.DueDate, Validators.required],
         EntryValue: [data.EntryValue, [Validators.required, Validators.min(0.01)]],
         Paid: [data.Paid],
         PayDate: [data.PayDate],
      });
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine)
         if (!await this.msg.Confirm('shared.ROLLBACK_TEXT', 'shared.ROLLBACK_CONFIRM_COMMAND', 'shared.ROLLBACK_ABORT_COMMAND'))
            return;
      this.router.navigate(["/entries/list"])
   }

   public async OnSaveClick() {
      if (!this.inputForm.valid)
         return;
      const data: EntryEntity = Object.assign(new EntryEntity, this.inputForm.value);
      if (!await this.service.SaveEntry(data))
         return;
      this.router.navigate(["/entries/list"])
   }

}
