import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenData } from '../token/token.data';
import { HttpClient } from '@angular/common/http';

@Injectable({
   providedIn: 'root'
})
export class TokenRefreshService {

   constructor(private http: HttpClient) { }

   public TokenRefresh(refreshToken: string): Observable<TokenData> {
      const authParam = Object.assign(new TokenAuthVM, {
         RefreshToken: refreshToken
      });
      return this.http.post<TokenData>(`api/identity/token-auth`, authParam);
   }

}

class TokenAuthVM {
   RefreshToken: string;
}
