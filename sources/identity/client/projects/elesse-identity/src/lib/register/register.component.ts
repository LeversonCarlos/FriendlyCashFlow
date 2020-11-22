import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityService } from '../identity.service';

@Component({
   selector: 'identity-register',
   templateUrl: './register.component.html',
   styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

   constructor(private service: IdentityService,
      private fb: FormBuilder) { }

   public inputForm: FormGroup;

   public ngOnInit(): void {
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         UserName: ['', [Validators.required, Validators.email]],
         Password: ['', Validators.required]
      });
   }

   public async OnClick() {
      if (!this.inputForm.valid)
         return;
      this.service.Register(this.inputForm.value.UserName, this.inputForm.value.Password);
   }

}
