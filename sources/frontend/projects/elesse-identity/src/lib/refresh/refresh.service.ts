import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenVM } from '@elesse/shared';
import { HttpClient } from '@angular/common/http';

@Injectable({
   providedIn: 'root'
})
export class RefreshService {

   constructor(private http: HttpClient) { }

   public TokenRefresh(refreshToken: string): Observable<TokenVM> {
      const authParam = Object.assign(new TokenAuthVM, {
         RefreshToken: refreshToken
      });
      return this.http.post<TokenVM>(`api/identity/token-auth`, authParam);
   }

}

class TokenAuthVM {
   RefreshToken: string;
}
