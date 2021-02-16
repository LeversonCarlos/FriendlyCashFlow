import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService } from '@elesse/shared';
import { TransfersData } from '../data/transfers.data';
import { TransferEntity } from '../model/transfers.model';

@Component({
   selector: 'transfers-details',
   templateUrl: './details.component.html',
   styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {

   constructor(private transferData: TransfersData,
      private busy: BusyService, private msg: MessageService,
      private activatedRoute: ActivatedRoute, private router: Router, private fb: FormBuilder) { }

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;

      this.data = await this.transferData.LoadTransfer(paramID);
      if (!this.data) {
         this.router.navigate(["/transactions/list"]);
         return;
      }

      this.form = this.fb.group({});
   }

   public data: TransferEntity;
   public form: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   public async OnCancelClick() {
      if (!this.form.pristine)
         if (!await this.msg.Confirm('shared.ROLLBACK_TEXT', 'shared.ROLLBACK_CONFIRM_COMMAND', 'shared.ROLLBACK_ABORT_COMMAND'))
            return;
      this.router.navigate(["/transactions/list"])
   }

   public async OnSaveClick() {
      if (!this.form.valid)
         return;
      if (!await this.transferData.SaveTransfer(this.data))
         return;
      this.router.navigate(["/transactions/list"])
   }

}
