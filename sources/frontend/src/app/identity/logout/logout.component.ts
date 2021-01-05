import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../token/token.service';
import { BusyService } from '@elesse/shared';

@Component({
   selector: 'identity-logout',
   templateUrl: './logout.component.html',
   styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {

   constructor(private tokenService: TokenService, private busy: BusyService,
      private router: Router) { }

   ngOnInit(): void {
      this.Logout();
   }

   private async Logout() {
      try {
         this.busy.show();

         this.tokenService.Token = null;
         this.router.navigateByUrl('/');

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
