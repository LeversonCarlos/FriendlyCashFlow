import { Injectable, Inject } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class UrlInterceptor implements HttpInterceptor {
   constructor(@Inject('BASE_URL') private baseUrl: string) {
   }
   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      const cloneUrl = `${this.baseUrl}${req.url}`;
      const cloneRequest = req.clone({ url: cloneUrl });
      return next.handle(cloneRequest);
   }
}

export const UrlInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: UrlInterceptor,
   multi: true
};

export const baseUrlProvider = {
   provide: 'BASE_URL',
   useFactory: () => { return document.getElementsByTagName('base')[0].href; },
   deps: []
};
