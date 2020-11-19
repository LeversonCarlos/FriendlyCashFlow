import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { SettingsService } from '../settings/settings.service';

@Injectable()
export class UrlInterceptor implements HttpInterceptor {

   constructor(private settings: SettingsService) { }

   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      if (!request.url.startsWith('api/'))
         return next.handle(request);
      else
         return this.settings.getSettings()
            .pipe(
               map(settings => settings.ApiBaseUrl as string),
               map(apiBaseUrl => `${apiBaseUrl}/${request.url}`),
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
