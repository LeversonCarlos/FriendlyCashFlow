import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
   providedIn: 'root'
})
export class AuthUnguard implements CanActivate {

   constructor(private auth: AuthService, private router: Router) { }

   canActivate(
      next: ActivatedRouteSnapshot,
      state: RouterStateSnapshot): boolean {
      if (!this.auth || !this.auth.Token || !this.auth.Token.UserID) { return true; }
      this.router.navigate(['/home']);
      return false
   }

}
