import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { TokenService } from 'elesse-shared';
import { catchError } from 'rxjs/operators';

@Injectable()
export class RequestAuthInterceptor implements HttpInterceptor {

   constructor(private tokenService: TokenService) { }

   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      if (this.tokenService.IsValid)
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

   constructor(private tokenService: TokenService) { }

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
      return null;
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
