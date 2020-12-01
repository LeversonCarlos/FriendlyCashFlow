import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { TokenService, TokenVM } from 'elesse-shared';
import { RefreshService } from 'elesse-identity';
import { catchError, filter, switchMap, take } from 'rxjs/operators';

@Injectable()
export class RequestAuthInterceptor implements HttpInterceptor {

   constructor(private tokenService: TokenService) { }

   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      if (this.tokenService.HasToken)
         request = request.clone({
            setHeaders: {
               Authorization: `Bearer ${this.tokenService.Token.AccessToken}`
            }
         });
      return next
         .handle(request);
   }

}

@Injectable()
export class ResponseAuthInterceptor implements HttpInterceptor {

   constructor(private tokenService: TokenService, private refreshService: RefreshService) { }

   private IsRefreshingToken: boolean = false;
   private RefreshingTokenEvent: BehaviorSubject<string> = new BehaviorSubject<string>(null);

   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      return next
         .handle(request)
         .pipe(
            catchError((error: HttpErrorResponse): Observable<HttpEvent<any>> => {
               if (error.status == 401)
                  return this.HandleUnauthorized(request, next);
               return throwError(error);
            })
         );
   }

   private HandleUnauthorized(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
      try {

         // IF HAS NO TOKEN, FORCE A LOCATION RELOAD AND THIS WILL SEND USER TO THE LOGIN PAGE
         if (!this.tokenService.HasToken) {
            this.tokenService.Token = null;
            location.reload();
            return of(null);
         }

         // IF IS ALREADY REFRESHING TOKEN, WAIT FOR IT THROUGH A BEHAVIOR SUBJECT
         if (this.IsRefreshingToken) {
            return this.RefreshingTokenEvent
               .pipe(
                  filter(token => token != null),
                  take(1),
                  switchMap(token => {
                     return this.RetryRequest(req, next);
                  })
               )
         }

         // ENTER IN REFRESHING STATE SO CONCURRENT INSTANCES WAIT FOR IT
         this.IsRefreshingToken = true;
         this.RefreshingTokenEvent.next(null);

         return this.refreshService
            .TokenRefresh(this.tokenService.Token.RefreshToken)
            .pipe(
               catchError((error): Observable<TokenVM> => {
                  this.tokenService.Token = null;
                  this.IsRefreshingToken = false
                  location.reload();
                  return of(null);
               }),
               switchMap(token => {
                  this.tokenService.Token = token;
                  this.RefreshingTokenEvent.next(this.tokenService.Token.AccessToken);
                  this.IsRefreshingToken = false
                  return this.RetryRequest(req, next);
               })
            );

      }
      catch (ex) { return of(null); }
   }

   private RetryRequest(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
      req = req.clone({
         setHeaders: {
            Authorization: `Bearer ${this.tokenService.Token.AccessToken}`
         }
      });
      return next.handle(req);
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
