import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BusyService, MessageService } from '@elesse/shared';
import { AccountsService } from '../../accounts.service';

@Component({
   selector: 'accounts-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private service: AccountsService,
      private busy: BusyService, private msg: MessageService,
      private fb: FormBuilder, private route: ActivatedRoute) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   async ngOnInit(): Promise<void> {
      const paramID = this.route.snapshot.params.id;
      const data = await this.service.LoadAccount(paramID);
      this.inputForm = this.fb.group({
         AccountID: [data.AccountID],
         Text: [data.Text, Validators.required],
         Type: [data.Type, Validators.required],
         ClosingDay: [data.ClosingDay],
         DueDay: [data.DueDay]
      });
   }

}
