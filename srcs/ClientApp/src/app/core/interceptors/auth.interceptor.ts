import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

   constructor(private auth: AuthService) { }

   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      req = this.GetRequestWithAuthorization(req);
      return next.handle(req).pipe(
         catchError(this.RequestError)
      );
   }

   private GetRequestWithAuthorization(req: HttpRequest<any>): HttpRequest<any> {
      if (this.auth && this.auth.Token && this.auth.Token.AccessToken) {
         req = req.clone({
            setHeaders: {
               Authorization: `Bearer ${this.auth.Token.AccessToken}`
            }
         });
      }
      return req;
   }

   private RequestError(error: HttpErrorResponse): Observable<never> {

      /* UNAUTHORIZED
      if (error.status == 401) {
         if (this.auth.Token && this.auth.Token.RefreshToken) {
            // this.auth.logout();
            // location.reload(true);
            // return;
         }
      }
      */

      return Observable.throw(error);
   }

}

export const AuthInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: AuthInterceptor,
   multi: true
};
