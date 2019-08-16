import { Injectable } from '@angular/core';
import { SignUp, User, SignIn } from './auth.models';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { MessageService } from 'src/app/shared/message/message.service';

@Injectable({
   providedIn: 'root'
})
export class AuthService {

   constructor(private busy: BusyService, private msg: MessageService,
      private http: HttpClient, private router: Router) { }

   public signupUser: User;
   public async signup(value: SignUp) {
      try {
         this.busy.show();
         this.signupUser = await this.http.post<User>(`api/users`, value).toPromise();
         if (this.signupUser && this.signupUser.UserID != null) {
            this.msg.ShowInfo('An email was sent to you with instructions on how to activate your account.')
            this.router.navigate(['/signin']);
         }
      }
      catch (ex) { console.error(ex); return null; }
      finally { this.busy.hide(); }
   }

   public async sendActivation(userID: string) {
      try {
         this.busy.show();
         const result = await this.http.post<boolean>(`api/users/sendActivation/${userID}`, null).toPromise();
         if (result) { this.msg.ShowInfo('An email was sent to you with instructions on how to activate your account.') }
      }
      catch (ex) { console.error(ex); return null; }
      finally { this.busy.hide(); }
   }

   public async activateUser(userID: string, activationCode: string) {
      try {
         this.busy.show();
         const result = await this.http.get<User>(`api/users/activate/${userID}/${encodeURIComponent(activationCode)}`).toPromise();
         if (result) { this.msg.ShowInfo('Your account has been activated, you can sign in now.') }
      }
      catch (ex) { console.error(ex); }
      finally { this.busy.hide(); this.router.navigate(['/signin']); }
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
