import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, of, BehaviorSubject } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
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
               return this.handleError401(req, next);
            }
            return throwError(error);
         })
      );
   }

   private handleRequest(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
      req = req.clone({
         setHeaders: {
            Authorization: `Bearer ${this.auth.Token.AccessToken}`
         }
      });
      return next.handle(req);
   }

   private isRefreshingToken: boolean = false;
   private tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);
   private handleError401(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
      try {

         // IF HAS NO TOKEN, FORCE A LOCATION RELOAD AND THIS WILL SEND USER TO THE LOGIN PAGE
         if (!this.auth.Token || !this.auth.Token.RefreshToken || this.auth.Token.RefreshToken == '') {
            this.auth.Token = null;
            location.reload(true);
            return of(null);
         }

         // IF IS ALREADY REFRESHING TOKEN, WAIT FOR IT THROUGH A BEHAVIOR SUBJECT
         if (this.isRefreshingToken) {
            this.appInsights.trackTrace('Token Expired: Will wait for the Refresh Instance', SeverityLevel.Information, {
               refreshToken: this.auth.Token.RefreshToken,
               requestedUrl: req.url,
               locationUrl: location.href
            });
            return this.tokenSubject
               .pipe(
                  filter(token => token != null),
                  take(1),
                  switchMap(token => {
                     return this.handleRequest(req, next);
                  })
               )
         }

         // ENTER IN REFRESHING STATE SO CONCURRENT INSTANCES WAIT FOR IT
         this.isRefreshingToken = true
         this.tokenSubject.next(null);
         this.appInsights.trackTrace('Token Expired: Will refresh it', SeverityLevel.Information, {
            refreshToken: this.auth.Token.RefreshToken,
            requestedUrl: req.url,
            locationUrl: location.href
         });

         return this.auth.signRefresh()
            .pipe(
               catchError((refreshError) => {
                  this.appInsights.trackTrace('Token Expired: Could not refresh', SeverityLevel.Warning, {
                     refreshToken: this.auth.Token.RefreshToken,
                     requestedUrl: req.url,
                     locationUrl: location.href,
                     refreshError: refreshError
                  })
                  this.auth.Token = null;
                  location.reload(true);
                  this.isRefreshingToken = false
                  return of(null);
               }),
               switchMap(() => {
                  this.tokenSubject.next(this.auth.Token.AccessToken);
                  this.isRefreshingToken = false
                  return this.handleRequest(req, next);
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
