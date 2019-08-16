import { Injectable } from '@angular/core';
import { SignUp, User, SignIn } from './auth.models';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
   providedIn: 'root'
})
export class AuthService {

   constructor(private busy: BusyService,
      private http: HttpClient, private router: Router) { }

   public signupUser: User;
   public async signup(value: SignUp) {
      try {
         this.busy.show();
         this.signupUser = await this.http.post<User>(`api/users`, value).toPromise();
         if (this.signupUser && this.signupUser.UserID != null) {
            this.router.navigate(['/signin']);
         }
      }
      catch (ex) { console.error(ex); return null; }
      finally { this.busy.hide(); }
   }

   public async signin(value: SignIn) {
      try {
         this.busy.show();
         console.log(value);
         // this.signupUser = await this.http.post<User>(`api/users`, value).toPromise();
      }
      catch (ex) { console.error(ex); return null; }
      finally { this.busy.hide(); }
   }

}
