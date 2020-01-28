import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, of, BehaviorSubject } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { AppInsightsService, SeverityLevel } from 'src/app/shared/app-insights/app-insights.service';

@Injectable()
export class RequestAuthInterceptor implements HttpInterceptor {
   constructor(private auth: AuthService) { }
   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      if (this.auth && this.auth.Token && this.auth.Token.AccessToken) {
         req = req.clone({
            setHeaders: {
               Authorization: `Bearer ${this.auth.Token.AccessToken}`
            }
         });
      }
      return next.handle(req);
   }
}

@Injectable()
export class ResponseAuthInterceptor implements HttpInterceptor {

   constructor(private auth: AuthService, private appInsights: AppInsightsService) { }

   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return next.handle(req).pipe(
         catchError((error: HttpErrorResponse) => {
            if (error.status == 401) {
               return this.intercept401(req, next);
            }
            return throwError(error);
         })
      );
   }

   private isRefreshingToken: boolean = false;
   private tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);
   private intercept401(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
      try {

         // IF HAS NO TOKEN, FORCE A LOCATION RELOAD AND THIS WILL SEND USER TO THE LOGIN PAGE
         if (!this.auth.Token || !this.auth.Token.RefreshToken || this.auth.Token.RefreshToken == '') {
            this.auth.Token = null;
            location.reload(true);
            return of(null);
         }

         this.appInsights.trackTrace('Token Expired: Will Try to Refresh', SeverityLevel.Information, {
            refreshToken: this.auth.Token.RefreshToken,
            requestedUrl: req.url,
            locationUrl: location.href
         });
         return this.auth.signRefresh()
            .pipe(
               catchError((refreshError) => {
                  this.appInsights.trackTrace('Token Expired: Could not Refresh', SeverityLevel.Warning, {
                     refreshToken: this.auth.Token.RefreshToken,
                     requestedUrl: req.url,
                     locationUrl: location.href,
                     refreshError: refreshError
                  })
                  this.auth.Token = null;
                  location.reload(true);
                  return of(null);
               }),
               switchMap(() => {
                  req = req.clone({
                     setHeaders: {
                        Authorization: `Bearer ${this.auth.Token.AccessToken}`
                     }
                  });
                  return next.handle(req);
               })
            );

      }
      catch (ex) { this.appInsights.trackException(ex); return of(null); }
   }

}

export const RequestAuthInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: RequestAuthInterceptor,
   multi: true
};

export const ResponseAuthInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: ResponseAuthInterceptor,
   multi: true
};
