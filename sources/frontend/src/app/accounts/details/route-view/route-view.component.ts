import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, MessageService } from '@elesse/shared';
import { AccountEntity, enAccountType } from '../../accounts.data';
import { AccountsService } from '../../accounts.service';

@Component({
   selector: 'accounts-details-route-view',
   templateUrl: './route-view.component.html',
   styleUrls: ['./route-view.component.scss']
})
export class DetailsRouteViewComponent implements OnInit {

   constructor(private service: AccountsService,
      private busy: BusyService, private msg: MessageService,
      private fb: FormBuilder, private activatedRoute: ActivatedRoute, private router: Router) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   public async ngOnInit(): Promise<void> {
      const paramID = this.activatedRoute.snapshot.params.id;
      const data = await this.service.LoadAccount(paramID);
      this.OnFormCreate(data);
   }

   private OnFormCreate(data: AccountEntity) {
      this.inputForm = this.fb.group({
         AccountID: [data?.AccountID],
         Text: [data?.Text, Validators.required],
         Type: [data?.Type, Validators.required],
         ClosingDay: new FormControl({ value: data?.ClosingDay, disabled: true }),
         DueDay: new FormControl({ value: data?.DueDay, disabled: true })
      });
      this.inputForm.controls['Type'].valueChanges.subscribe(type => {
         this.OnAccountTypeChanged(type, 'ClosingDay');
         this.OnAccountTypeChanged(type, 'DueDay');
      });
      this.OnAccountTypeChanged(data?.Type, 'ClosingDay');
      this.OnAccountTypeChanged(data?.Type, 'DueDay');
   }

   private OnAccountTypeChanged(type: enAccountType, controlName: string) {
      const control = this.inputForm.controls[controlName];
      if (type == enAccountType.CreditCard) {
         control.enable();
         control.setValidators([Validators.required, Validators.min(1), Validators.max(31)]);
         control.markAsTouched();
      }
      else {
         control.clearValidators();
         control.markAsUntouched();
         control.setValue('');
         control.disable();
      }
      control.updateValueAndValidity();
   }

   public async OnCancelClick() {
      if (!this.inputForm.pristine)
         //if (!await this.msg.Confirm('BASE_CANCEL_CHANGES_CONFIRMATION_TEXT', 'BASE_CANCEL_CHANGES_CONFIRM', 'BASE_CANCEL_CHANGES_ABORT'))
         return;
      this.router.navigateByUrl("../list")
   }

   public async OnSaveClick() {
      // if (!await this.service.saveAccount(this.Data))
      // return;
      this.router.navigate(["/accounts/list"])
   }

}
