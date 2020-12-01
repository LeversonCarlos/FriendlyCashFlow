import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BusyService } from 'elesse-shared';
import { IdentityService } from '../identity.service';

@Component({
   selector: 'identity-login',
   templateUrl: './login.component.html',
   styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

   constructor(private service: IdentityService, private busy: BusyService,
      private fb: FormBuilder, private activatedRoute: ActivatedRoute) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }
   private returnUrl: string;

   public ngOnInit(): void {
      this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/';
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         UserName: ['', [Validators.required, Validators.email]],
         Password: ['', Validators.required]
      });
   }

   public OnClick() {
      if (!this.inputForm.valid)
         return;
      this.service.Login(this.inputForm.value.UserName, this.inputForm.value.Password, this.returnUrl);
   }

}
