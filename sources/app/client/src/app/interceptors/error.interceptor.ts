import { Injectable, Injector } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { InsightsService, MessageService } from 'elesse-shared';
import { MessageData, MessageDataType } from 'elesse-shared';

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

               // UNESPECTED RESULT FROM API
               this.injector.get<InsightsService>(InsightsService).TrackEvent('Result from Backend', { error: error });
               if (!error.error) {
                  console.error(' UNESPECTED RESULT FROM API', error); return;
               }

               // SHOW API MESSAGE ON SCREEN
               this.injector.get<MessageService>(MessageService).ShowMessage(this.GetMessage(error.error));
               return throwError(error);

            })
         );
   }

   private GetMessage(errorList: string[]): MessageData {
      let msg = new MessageData();
      msg.Messages = [];
      errorList.forEach(errorKey => {

         if (errorKey.lastIndexOf("WARNING_", 0) == 0)
            msg.Type = MessageDataType.Warning;
         else if (errorKey.lastIndexOf("EXCEPTION_", 0) == 0)
            // msg.Details = JSON.stringify(error);
            msg.Type = MessageDataType.Error;
         else if (errorKey.lastIndexOf("INNER_EXCEPTION_", 0) == 0)
            msg.Type = MessageDataType.Error;
         else
            msg.Type = MessageDataType.Information;
         msg.Messages.push(errorKey);

      });
      return msg;
   }

}

export const ErrorInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: ErrorInterceptor,
   multi: true
};
