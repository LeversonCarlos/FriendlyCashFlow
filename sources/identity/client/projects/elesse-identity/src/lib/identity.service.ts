import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BusyService, TokenService } from 'elesse-shared';

@Injectable({
   providedIn: 'root'
})
export class IdentityService {

   constructor(private tokenService: TokenService, private busy: BusyService,
      private router: Router) { }

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
