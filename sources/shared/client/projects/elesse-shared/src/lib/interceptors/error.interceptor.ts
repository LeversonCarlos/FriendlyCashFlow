import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MessageService } from '../message/message.service';
import { tap } from 'rxjs/operators';
import { MessageData, MessageDataType } from '../message/message.models';
import { InsightsService } from '../insights/insights.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

   constructor(private msg: MessageService, private insights: InsightsService) { }

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
                     this.insights.TrackException(error); return;
                  }

                  // SHOW API MESSAGE ON SCREEN
                  this.msg.ShowMessage(this.GetMessage(error.error));

               }
            )
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
