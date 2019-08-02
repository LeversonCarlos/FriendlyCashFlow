
import { Injectable, Inject } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
   constructor() { }
   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return next.handle(req)
         .pipe(
            tap(
               (next: HttpEvent<any>) => { },
               (error: HttpErrorResponse) => {
                  if (!error || !error.status || error.status == 200) { return; }
                  if (req.url.includes('api/translations')) { return; }

                  /* TODO
                  if (error.status == 401) {
                     this.auth.logout();
                     location.reload(true);
                     return;
                  }
                  */
                  console.error('intereptor', error);

               }
            )
         );
   }
}

export const ErrorInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: ErrorInterceptor,
   multi: true
};

