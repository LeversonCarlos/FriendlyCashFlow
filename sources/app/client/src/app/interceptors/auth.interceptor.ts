import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenService } from 'elesse-shared';

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

export const RequestAuthInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: RequestAuthInterceptor,
   multi: true
};
