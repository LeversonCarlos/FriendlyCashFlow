import { Injectable } from '@angular/core';

export class Token {
   UserID: string;
   AccessToken: string;
   RefreshToken: string;
}

@Injectable({
   providedIn: 'root'
})
export class TokenService {

   constructor() { }

   private tokenTag: string = 'currentToken';

   public get Data(): Token {
      try {
         return JSON.parse(localStorage.getItem(this.tokenTag))
      } catch (error) { return null }
   }

   public set Data(value: Token) {
      try {
         if (!value) { localStorage.removeItem(this.tokenTag); }
         else {
            localStorage.setItem(this.tokenTag, JSON.stringify(value))
         }
      } catch (error) { }
   }

}
