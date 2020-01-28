
import { Injectable, Inject } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HTTP_INTERCEPTORS, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MessageData, MessageDataType } from 'src/app/shared/message/message.models';
import { MessageService } from 'src/app/shared/message/message.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
   constructor(private msg: MessageService) { }

   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return next.handle(req)
         .pipe(
            tap(
               (next: HttpEvent<any>) => { },
               (error: HttpErrorResponse) => {
                  if (!error || !error.status || error.status == 200) { return; }
                  if (req.url.includes('api/translations')) { return; }

                  // FORBIDDEN
                  if (error.status == 403) {
                     this.msg.ShowInfo("BASE_FORBIDDEN_MESSAGE");
                     return;
                  }

                  // UNAUTHORIZED
                  if (error.status == 401) {
                     return;
                  }

                  if (!error.error) { console.error('ErrorInterceptor', error); return; }
                  const message = this.GetMessage(error.error);
                  this.msg.ShowMessage(message);

               }
            )
         );
   }

   private GetMessage(error: any): MessageData {
      let msg = new MessageData();
      msg.Messages = [];
      Object.keys(error).forEach(errorKey => {

         const errorValues: string[] = error[errorKey];
         errorValues.forEach(errorValue => {

            if (errorKey.lastIndexOf("WARNING_", 0) == 0) {
               msg.Type = MessageDataType.Warning;
               msg.Messages.push(errorValue);
            }
            else if (errorKey.lastIndexOf("EXCEPTION_", 0) == 0) {
               msg.Type = MessageDataType.Error;
               msg.Messages.push(errorValue);
               msg.Details = JSON.stringify(error);
            }
            else if (errorKey.lastIndexOf("INNER_EXCEPTION_", 0) == 0) {
               msg.Type = MessageDataType.Error;
               msg.Messages.push(errorValue);
            }
            else {
               msg.Type = MessageDataType.Information;
               msg.Messages.push(errorValue);
            }

         });

      });
      return msg;
   }

}

export const ErrorInterceptorProvider = {
   provide: HTTP_INTERCEPTORS,
   useClass: ErrorInterceptor,
   multi: true
};

