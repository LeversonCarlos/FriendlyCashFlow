import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BusyService, MessageService } from '@elesse/shared';

@Component({
   selector: 'identity-change-password',
   templateUrl: './change-password.component.html',
   styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

   constructor(private busy: BusyService, private msg: MessageService,
      private fb: FormBuilder, private http: HttpClient, private router: Router) { }

   public inputForm: FormGroup;
   public get IsBusy(): boolean { return this.busy.IsBusy; }

   ngOnInit(): void {
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         OldPassword: ['', [Validators.required]],
         NewPassword: ['', Validators.required]
      });
   }

   public async OnFormSubmit() {
      try {
         if (!this.inputForm.valid)
            return;
         this.busy.show();

         const changePasswordParam = Object.assign(new ChangePasswordVM, {
            OldPassword: this.inputForm.value.OldPassword,
            NewPassword: this.inputForm.value.NewPassword
         });
         await this.http.put(`api/identity/change-password`, changePasswordParam).toPromise();

         await this.msg.ShowInfo('identity.CHANGE_PASSWORD_SUCCESS')
         this.router.navigateByUrl('/');

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}

class ChangePasswordVM {
   OldPassword: string;
   NewPassword: string;
}
