import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TokenService } from '../token/token.service';
import { BusyService } from '@elesse/shared';
import { TokenData } from '../token/token.data';

@Component({
   selector: 'identity-login',
   templateUrl: './login.component.html',
   styleUrls: ['./login.component.scss']
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

   public async OnFormSubmit() {
      try {
         if (!this.inputForm.valid)
            return;
         this.busy.show();

         const authParam = Object.assign(new UserAuthVM, {
            UserName: this.inputForm.value.UserName,
            Password: this.inputForm.value.Password
         });
         this.tokenService.Token = await this.http.post<TokenData>(`api/identity/user-auth`, authParam).toPromise();

         if (this.tokenService.HasToken)
            this.router.navigateByUrl(this.returnUrl);

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public OnActivationClick() {
      console.error('TODO: PRECISAMOS IMPLEMENTAR ISSO AQUI')
   }

}

class UserAuthVM {
   UserName: string;
   Password: string;
}
