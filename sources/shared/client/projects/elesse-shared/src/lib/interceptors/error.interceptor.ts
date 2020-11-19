import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MessageService } from '../message/message.service';
import { tap } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

   constructor(private msg: MessageService) { }

   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      return next
         .handle(request)
         .pipe(
            tap(
               (next: HttpEvent<any>) => { },
               (error: HttpErrorResponse) => {

                  // SUCCESS RESULT
                  if (!error || !error.status || error.status == 200)
                     return;

                  // TRANSLATION NOT FOUND
                  // if (request.url.includes('api/translations'))
                  //   return;

                  // UNAUTHORIZED WILL FALL INTO TOKEN LIFE CICLE
                  if (error.status == 401)
                     return;

                  // SPECIFIC MESSAGE FOR FORBIDDEN ACCESS
                  if (error.status == 403) {
                     this.msg.ShowInfo("SHARED_FORBIDDEN_MESSAGE");
                     return;
                  }

                  // UNESPECTED RESULT FROM API
                  if (!error.error) {
                     console.error('Unespected Result from API', error); return;
                  }

                  // SHOW API MESSAGE ON SCREEN
                  this.msg.ShowMessage(this.GetMessage(error.error));

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
