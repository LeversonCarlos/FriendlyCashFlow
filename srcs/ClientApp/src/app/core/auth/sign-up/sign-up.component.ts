import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { BusyService } from 'src/app/shared/busy/busy.service';

@Component({
   selector: 'fs-sign-up',
   templateUrl: './sign-up.component.html',
   styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {

   constructor(private service: AuthService, public busy: BusyService,
      private fb: FormBuilder) { }

   public inputForm: FormGroup;

   public ngOnInit() {
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         Description: ['', Validators.required],
         UserName: ['', [Validators.required, Validators.email]],
         Password: ['', Validators.required]
      });
   }

   public OnClick() {
      if (!this.inputForm.valid) { return; }
      this.service.signUp(this.inputForm.value);
   }

}
