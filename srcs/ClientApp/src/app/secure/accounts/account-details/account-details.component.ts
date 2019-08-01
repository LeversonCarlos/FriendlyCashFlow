import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsService, Account } from '../accounts.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({
   selector: 'fs-account-details',
   templateUrl: './account-details.component.html',
   styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {

   constructor(private service: AccountsService,
      private route: ActivatedRoute, private fb: FormBuilder) { }

   public Data: Account;
   public inputForm: FormGroup;

   public async ngOnInit() {
      const accountID: number = Number(this.route.snapshot.params.id)
      if (!accountID || accountID == 0) { console.error('todo'); return; }

      this.Data = await this.service.getAccount(accountID);
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Text: [this.Data.Text, Validators.required],
         Type: [this.Data.Type, Validators.required],
         DueDay: [this.Data.DueDay],
         Active: [this.Data.Active, Validators.required]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.Text = values.Text || '';
         this.Data.Type = values.Type || 0;
         this.Data.DueDay = values.DueDay;
         this.Data.Active = values.Type || false;
      });
      this.inputForm.dirty
   }

}
