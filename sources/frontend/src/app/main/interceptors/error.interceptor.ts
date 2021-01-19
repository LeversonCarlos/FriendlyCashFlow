import { Injectable, Injector } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { InsightsService, MessageService } from '@elesse/shared';
import { MessageData, MessageType } from '@elesse/shared';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

   constructor(private injector: Injector) { }

   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      return next
         .handle(request)
         .pipe(
            catchError((error: HttpErrorResponse): Observable<HttpEvent<any>> => {

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
                  this.injector.get<MessageService>(MessageService).ShowInfo("SHARED_FORBIDDEN_MESSAGE");
                  return;
               }

               // SHOW API MESSAGE ON SCREEN
               if (error.error) {
                  const IsMessageData = (obj: any): obj is MessageData => {
                     return obj.Type !== undefined;
                  }
                  let messageData = Object.assign(new MessageData, error.error);
                  if (!IsMessageData(error.error))
                     messageData = Object.assign(new MessageData, { Type: MessageType.Exception, Details: error.error });
                  this.injector.get<MessageService>(MessageService).ShowMessages(messageData, true);
               }

               // UNESPECTED RESULT FROM API
               if (!error.error) {
                  this.injector.get<InsightsService>(InsightsService).TrackEvent('UNESPECTED RESULT FROM API', { error: error });
                  console.error('UNESPECTED RESULT FROM API', error); return;
               }

               return throwError(error);
            })
         );
   }

}

export const ErrorInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: ErrorInterceptor,
   multi: true
};
