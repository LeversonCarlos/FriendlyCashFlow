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
               if (error.error)
                  this.injector.get<MessageService>(MessageService).ShowMessages(this.GetMessage(error.error), true);

               // UNESPECTED RESULT FROM API
               if (!error.error) {
                  this.injector.get<InsightsService>(InsightsService).TrackEvent('UNESPECTED RESULT FROM API', { error: error });
                  console.error('UNESPECTED RESULT FROM API', error); return;
               }

               return throwError(error);
            })
         );
   }

   private GetMessage(backendMessageList: BackendMessage[]): MessageData {
      let msg = new MessageData();
      msg.Messages = [];
      backendMessageList.forEach(backendMessage => {

         if (backendMessage.Type == enBackendMessageType.Warning)
            msg.Type = MessageType.Warning;
         else if (backendMessage.Type == enBackendMessageType.Error)
            // msg.Details = JSON.stringify(error);
            msg.Type = MessageType.Error;
         else
            msg.Type = MessageType.Information;
         msg.Messages.push(backendMessage.Text);

      });
      return msg;
   }

}

enum enBackendMessageType { Info = 0, Warning = 1, Error = 2 };
class BackendMessage {
   Type: enBackendMessageType;
   Text: string;
}

export const ErrorInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: ErrorInterceptor,
   multi: true
};
