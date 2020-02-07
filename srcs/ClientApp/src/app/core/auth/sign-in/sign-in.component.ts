import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { User, SignIn } from '../auth.models';
import { ActivatedRoute } from '@angular/router';
import { BusyService } from 'src/app/shared/busy/busy.service';

@Component({
   selector: 'fs-sign-in',
   templateUrl: './sign-in.component.html',
   styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

   constructor(private service: AuthService, public busy: BusyService,
      private activatedRoute: ActivatedRoute, private fb: FormBuilder) { }

   public get signupUser(): User { return this.service && this.service.signupUser; }
   public inputForm: FormGroup;
   private returnUrl: string;

   public ngOnInit() {
      this.OnFormCreate();
      this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/home';
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         UserName: ['', [Validators.required, Validators.email]],
         Password: ['', Validators.required]
      });
   }

   public OnClick() {
      if (!this.inputForm.valid) { return; }
      this.service.signIn(Object.assign(new SignIn, this.inputForm.value), this.returnUrl);
   }

   public OnActivationClick() {
      this.service.sendActivation(this.signupUser.UserID);
   }

}
