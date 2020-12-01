import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusyService, TokenService, TokenVM } from 'elesse-shared';

@Component({
   selector: 'identity-login',
   templateUrl: './login.component.html',
   styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

   constructor(private tokenService: TokenService, private busy: BusyService,
      private fb: FormBuilder, private activatedRoute: ActivatedRoute, private http: HttpClient, private router: Router) { }

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

   public OnFormSubmit() {
      if (!this.inputForm.valid)
         return;
      this.Login(this.inputForm.value.UserName, this.inputForm.value.Password, this.returnUrl);
   }

   private async Login(userName: string, password: string, returnUrl: string) {
      try {
         this.busy.show();

         const authParam = Object.assign(new UserAuthVM, {
            UserName: userName,
            Password: password
         });
         this.tokenService.Token = await this.http.post<TokenVM>(`api/identity/user-auth`, authParam).toPromise();

         if (this.tokenService.HasToken)
            this.router.navigateByUrl(returnUrl);

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}

class UserAuthVM {
   UserName: string;
   Password: string;
}
