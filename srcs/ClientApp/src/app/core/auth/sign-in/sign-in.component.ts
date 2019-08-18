import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { User, SignIn } from '../auth.models';

@Component({
   selector: 'fs-sign-in',
   templateUrl: './sign-in.component.html',
   styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

   constructor(private service: AuthService, private fb: FormBuilder) { }

   public get signupUser(): User { return this.service && this.service.signupUser; }
   public inputForm: FormGroup;

   public ngOnInit() {
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         UserName: ['', [Validators.required, Validators.email]],
         Password: ['', Validators.required]
      });
   }

   public OnClick() {
      if (!this.inputForm.valid) { return; }
      this.service.signin(Object.assign(new SignIn, this.inputForm.value));
   }

   public OnActivationClick() {
      this.service.sendActivation(this.signupUser.UserID);
   }

}
