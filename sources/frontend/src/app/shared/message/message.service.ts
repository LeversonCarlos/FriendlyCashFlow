import { Injectable } from '@angular/core';
import { MatSnackBar } from '@elesse/material';
import { MessageViewComponent } from './message-view/message-view.component';
import { MessageData, MessageType } from './message.models';

@Injectable({
   providedIn: 'root'
})
export class MessageService {

   constructor(private snackBar: MatSnackBar) { }
   // TODO: constructor(private injector: Injector) { }

   ShowMessage(messageData: MessageData): void {
      this.snackBar.openFromComponent(MessageViewComponent, {
         panelClass: 'message-snack-panel',
         data: messageData,
         duration: messageData.Duration,
         horizontalPosition: 'right',
         verticalPosition: 'top'
      });
   }

   public async ShowInfo(...messages: string[]): Promise<void> {
      if (!messages || messages.length == 0)
         return;
      // TODO: const translatedMessage = await Promise.all(messages.map(async message => await this.translation.getValue(message))); }
      const translatedMessage = messages;
      const messageData = Object.assign(new MessageData, {
         Messages: translatedMessage,
         Type: MessageType.Information
      });
      this.ShowMessage(messageData);
   }

   public async ShowWarning(...messages: string[]): Promise<void> {
      if (!messages || messages.length == 0)
         return;
      // TODO: const translatedMessage = await Promise.all(messages.map(async message => await this.translation.getValue(message))); }
      const translatedMessage = messages;
      let messageData = Object.assign(new MessageData, {
         Messages: translatedMessage,
         Type: MessageType.Warning
      });
      this.ShowMessage(messageData);
   }

   public async ShowError(messages: string[], details: string): Promise<void> {
      const messageData = Object.assign(new MessageData, {
         Messages: messages,
         Details: details,
         Type: MessageType.Error
      });
      this.ShowMessage(messageData);
   }

   public async ShowException(ex: any): Promise<void> {
      const translatedMessage = 'SHARED_EXCEPTION_MESSAGE';
      const messageData = Object.assign(new MessageData, {
         Messages: translatedMessage,
         Details: ex,
         Type: MessageType.Error
      });
      this.ShowMessage(messageData);
      // TODO: this.injector.get<InsightsService>(InsightsService).TrackError(ex);
   }

}
