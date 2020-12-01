import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BusyService, MessageService, TokenService, TokenVM } from 'elesse-shared';

@Injectable({
   providedIn: 'root'
})
export class IdentityService {

   constructor(private tokenService: TokenService, private busy: BusyService, private msg: MessageService,
      private http: HttpClient, private router: Router) { }

   public async Login(userName: string, password: string, returnUrl: string) {
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

   public async Logout() {
      try {
         this.busy.show();

         this.tokenService.Token = null;
         this.router.navigateByUrl('/');

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}

class UserAuthVM {
   UserName: string;
   Password: string;
}
