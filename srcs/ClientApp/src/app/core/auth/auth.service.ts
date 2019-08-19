import { Injectable } from '@angular/core';
import { SignUp, User, SignIn } from './auth.models';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { MessageService } from 'src/app/shared/message/message.service';
import { Token, TokenService } from './token.service';

@Injectable({
   providedIn: 'root'
})
export class AuthService {

   constructor(private busy: BusyService, private msg: MessageService, private token: TokenService,
      private http: HttpClient, private router: Router) { }

   public signupUser: User;
   public async signup(value: SignUp) {
      try {
         this.busy.show();
         this.signupUser = await this.http.post<User>(`api/users`, value).toPromise();
         if (this.signupUser && this.signupUser.UserID != null) {
            await this.msg.ShowInfo('USERS_ACTIVATION_INSTRUCTIONS_WAS_SENT_MESSAGE')
            this.router.navigate(['/signin']);
         }
      }
      catch (ex) { console.error(ex); }
      finally { this.busy.hide(); }
   }

   public async sendActivation(userID: string) {
      try {
         this.busy.show();
         const result = await this.http.post<boolean>(`api/users/sendActivation/${userID}`, null).toPromise();
         if (result) { await this.msg.ShowInfo('USERS_ACTIVATION_INSTRUCTIONS_WAS_SENT_MESSAGE') }
      }
      catch (ex) { console.error(ex); }
      finally { this.busy.hide(); }
   }

   public async activateUser(userID: string, activationCode: string) {
      try {
         this.busy.show();
         const result = await this.http.get<User>(`api/users/activate/${userID}/${encodeURIComponent(activationCode)}`).toPromise();
         if (result) { await this.msg.ShowInfo('USERS_ACCOUNT_HAS_BEEN_ACTIVATED_MESSAGE'); }
      }
      catch (ex) { console.error(ex); }
      finally { this.busy.hide(); this.router.navigate(['/signin']); }
   }

   public async signin(value: SignIn, returnUrl: string): Promise<boolean> {
      try {
         this.busy.show();
         this.token.Data = await this.http.post<Token>(`api/users/auth`, value).toPromise();
         if (this.token.Data && this.token.Data.UserID != null) {
            this.router.navigateByUrl(returnUrl);
         }
      }
      catch (ex) { console.error(ex); return false; }
      finally { this.busy.hide(); }
   }

}
