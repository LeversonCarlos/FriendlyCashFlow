import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BusyService, MessageService, TokenVM } from 'elesse-shared';

@Injectable({
   providedIn: 'root'
})
export class ElesseIdentityService {

   constructor(private busy: BusyService, private msg: MessageService,
      private http: HttpClient, private router: Router) { }

   public async Register(userName: string, password: string) {
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

   public async TokenAuth(refreshToken: string) {
      try {
         this.busy.show();

         const authParam = Object.assign(new TokenAuthVM, {
            RefreshToken: refreshToken
         });
         const authResult = await this.http.post<TokenVM>(`api/identity/token-auth`, authParam).toPromise();

         return authResult;
      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}

class RegisterVM {
   UserName: string;
   Password: string;
}

class TokenAuthVM {
   RefreshToken: string;
}
