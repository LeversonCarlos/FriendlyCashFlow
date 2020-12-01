import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BusyService, MessageService } from 'elesse-shared';

@Component({
   selector: 'identity-register',
   templateUrl: './register.component.html',
   styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

   constructor(private busy: BusyService, private msg: MessageService,
      private fb: FormBuilder, private http: HttpClient, private router: Router) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   public ngOnInit(): void {
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         UserName: ['', [Validators.required, Validators.email]],
         Password: ['', Validators.required]
      });
   }

   public OnFormSubmit() {
      if (!this.inputForm.valid)
         return;
      this.Register(this.inputForm.value.UserName, this.inputForm.value.Password);
   }

   private async Register(userName: string, password: string) {
      try {
         this.busy.show();

         const registerParam = Object.assign(new RegisterVM, {
            UserName: userName,
            Password: password
         });
         await this.http.post<boolean>(`api/identity/register`, registerParam).toPromise();

         await this.msg.ShowInfo('IDENTITY_REGISTER_SUCCESS_MESSAGE')
         this.router.navigate(['/'], { queryParamsHandling: 'preserve' });
      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}

class RegisterVM {
   UserName: string;
   Password: string;
}
