import { Injectable } from '@angular/core';
import { SignUp, User, SignIn, SignRefresh, Token } from './auth.models';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { MessageService } from 'src/app/shared/message/message.service';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
   providedIn: 'root'
})
export class AuthService {

   constructor(private busy: BusyService, private msg: MessageService,
      private http: HttpClient, private router: Router) { }


   /* SIGN UP */
   public signupUser: User;
   public async signUp(value: SignUp) {
      try {
         this.busy.show();
         this.signupUser = await this.http.post<User>(`api/users`, value).toPromise();
         if (this.signupUser && this.signupUser.UserID != null) {
            await this.msg.ShowInfo('USERS_ACTIVATION_INSTRUCTIONS_WAS_SENT_MESSAGE')
            this.router.navigate(['/auth/signin']);
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
         const url = `api/users/activate/${userID}/${encodeURIComponent(activationCode)}`;
         const result = await this.http.get<User>(url).toPromise();
         if (result) { await this.msg.ShowInfo('USERS_ACCOUNT_HAS_BEEN_ACTIVATED_MESSAGE'); }
      }
      catch (ex) { console.error(ex); }
      finally { this.busy.hide(); this.router.navigate(['/auth/signin']); }
   }


   /* TOKEN */
   private tokenTag: string = 'currentToken';
   public get Token(): Token {
      try {
         return JSON.parse(localStorage.getItem(this.tokenTag))
      } catch (error) { return null }
   }
   public set Token(value: Token) {
      try {
         if (!value) { localStorage.removeItem(this.tokenTag); }
         else {
            localStorage.setItem(this.tokenTag, JSON.stringify(value))
         }
      } catch (error) { }
   }


   /* SIGN IN */
   public async signIn(value: SignIn, returnUrl: string): Promise<boolean> {
      try {
         this.busy.show();
         this.Token = await this.http.post<Token>(`api/users/auth`, value).toPromise();
         if (this.Token && this.Token.UserID != null) {
            this.router.navigateByUrl(returnUrl);
         }
      }
      catch (ex) { console.error(ex); return false; }
      finally { this.busy.hide(); }
   }


   /* SIGN REFRESH */
   public signRefresh(): Observable<Token> {
      try {
         return this.http
            .post<Token>(`api/users/auth`, Object.assign(new SignRefresh, { RefreshToken: this.Token.RefreshToken }))
            .pipe(tap(x => this.Token = x));
      }
      catch (ex) { console.error(ex); return null; }
   }


   /* SIGN OUT */
   public async signOut() {
      try {
         this.Token = null;
         localStorage.clear();
         // this.router.navigate(['/home']);
         window.location = window.location
      }
      catch (ex) { console.error(ex); }
   }


}
