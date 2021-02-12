import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocaleService } from '@elesse/shared';

@Injectable()
export class LangInterceptor implements HttpInterceptor {

   constructor(private localeService: LocaleService) { }

   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      request = request.clone({
         setHeaders: {
            'Accept-Language': this.localeService.Language
         }
      });
      return next
         .handle(request);
   }

}

export const LangInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: LangInterceptor,
   multi: true
};

