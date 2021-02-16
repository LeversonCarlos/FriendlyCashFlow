import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { BusyService, MessageService } from '@elesse/shared';

@Component({
   selector: 'transfers-details-cancel',
   templateUrl: './cancel.component.html',
   styleUrls: ['./cancel.component.scss']
})
export class CancelComponent implements OnInit {

   constructor(
      private busy: BusyService, private msg: MessageService,
      private router: Router) { }

   ngOnInit(): void {
   }

   @Input() form: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   public async OnCancelClick() {
      if (!this.form.pristine)
         if (!await this.msg.Confirm('shared.ROLLBACK_TEXT', 'shared.ROLLBACK_CONFIRM_COMMAND', 'shared.ROLLBACK_ABORT_COMMAND'))
            return;
      this.router.navigate(["/transactions/list"])
   }

}
