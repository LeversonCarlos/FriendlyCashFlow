import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { TokenService } from '../token/token.service';

@Injectable({
   providedIn: 'root'
})
export class AuthGuard implements CanActivate {

   constructor(private tokenService: TokenService, private router: Router) { }

   canActivate(
      route: ActivatedRouteSnapshot,
      state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      return this.canActivateHandler(state.url);
   }

   private canActivateHandler(returnUrl: string): boolean {
      if (this.tokenService.HasToken)
         return true;
      this.router.navigate(['/identity/login'], { queryParams: { returnUrl: returnUrl } });
      return false
   }

}
