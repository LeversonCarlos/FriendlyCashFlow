import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { TokenService } from './token.service';


@Injectable({
   providedIn: 'root'
})
export class AuthGuard implements CanActivate {

   constructor(private token: TokenService, private router: Router) { }

   canActivate(
      next: ActivatedRouteSnapshot,
      state: RouterStateSnapshot): boolean {
      if (this.token && this.token.Data && this.token.Data.UserID) { return true; }
      this.router.navigate(['/signin'], { queryParams: { returnUrl: state.url } });
      return false
   }

}
