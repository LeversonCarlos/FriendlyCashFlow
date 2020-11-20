import { Injectable, Injector } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { SettingsService } from 'elesse-shared';

@Injectable()
export class UrlInterceptor implements HttpInterceptor {

   constructor(private injector: Injector) { }

   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      if (!request.url.startsWith('api/'))
         return next.handle(request);
      else
         return this.injector.get<SettingsService>(SettingsService)
            .getSettings()
            .pipe(
               map(settings => settings.Backend.Url as string),
               map(backendUrl => `${backendUrl}/${request.url}`),
               map(fullUrl => request.clone({ url: fullUrl })),
               switchMap(cloneRequest => next.handle(cloneRequest))
            );
   }
}

export const UrlInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: UrlInterceptor,
   multi: true
};
