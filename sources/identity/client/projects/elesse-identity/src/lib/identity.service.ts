import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BusyService, MessageService, TokenVM } from 'elesse-shared';
import { Observable } from 'rxjs';

@Injectable({
   providedIn: 'root'
})
export class IdentityService {

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

   public UserAuth(userName: string, password: string): Observable<TokenVM> {
      const authParam = Object.assign(new UserAuthVM, {
         UserName: userName,
         Password: password
      });
      return this.http.post<TokenVM>(`api/identity/user-auth`, authParam);
   }

   public TokenAuth(refreshToken: string): Observable<TokenVM> {
      const authParam = Object.assign(new TokenAuthVM, {
         RefreshToken: refreshToken
      });
      return this.http.post<TokenVM>(`api/identity/token-auth`, authParam);
   }

}

class RegisterVM {
   UserName: string;
   Password: string;
}

class UserAuthVM {
   UserName: string;
   Password: string;
}

class TokenAuthVM {
   RefreshToken: string;
}
