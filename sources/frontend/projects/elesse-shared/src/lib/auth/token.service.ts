import { Injectable } from '@angular/core';
import { TokenVM } from './token.models';

@Injectable({
   providedIn: 'root'
})
export class TokenService {

   constructor() { }

   private tokenTag: string = 'currentToken';

   public get Token(): TokenVM {
      try {
         return JSON.parse(localStorage.getItem(this.tokenTag))
      }
      catch (error) { return null; }
   }

   public set Token(value: TokenVM) {
      try {
         if (!value)
            localStorage.removeItem(this.tokenTag);
         else
            localStorage.setItem(this.tokenTag, JSON.stringify(value))
      }
      catch (error) { }
   }

   public get HasToken(): boolean {
      if (this.Token && this.Token.UserID && this.Token.AccessToken && this.Token.AccessToken != '')
         return true;
      else
         return false;
   }

}
