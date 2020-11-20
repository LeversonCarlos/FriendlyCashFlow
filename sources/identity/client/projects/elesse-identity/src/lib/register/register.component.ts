import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BusyService, MessageService } from 'elesse-shared';
import { Router } from '@angular/router';

@Component({
   selector: 'identity-register',
   templateUrl: './register.component.html',
   styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

   constructor(private busy: BusyService, private msg: MessageService,
      private fb: FormBuilder, private http: HttpClient, private router: Router) { }

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
      try {
         if (!this.inputForm.valid)
            return;
         this.busy.show();

         const registerParam = Object.assign(new RegisterVM, {
            UserName: this.inputForm.value.UserName,
            Password: this.inputForm.value.Password
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
