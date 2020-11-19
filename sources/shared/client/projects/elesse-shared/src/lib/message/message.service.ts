import { Injectable } from '@angular/core';
import { MessageData, MessageDataType } from './message.models';

@Injectable({
   providedIn: 'root'
})
export class MessageService {

   constructor() { }

   ShowMessage(messageData: MessageData): void {
      /*
      TODO
      this.snackBar.openFromComponent(MessageComponent, {
         panelClass: 'message-snack-panel',
         data: data,
         duration: data.Duration,
         horizontalPosition: 'right',
         verticalPosition: 'top'
      });
      */
      console.log('ShowMessage', messageData);
   }

   public async ShowInfo(...messages: string[]): Promise<void> {
      if (!messages || messages.length == 0)
         return;
      // const translatedMessage = await Promise.all(messages.map(async message => await this.translation.getValue(message))); }
      const translatedMessage = messages;
      const messageData = Object.assign(new MessageData, {
         Messages: translatedMessage,
         Type: MessageDataType.Information
      });
      this.ShowMessage(messageData);
   }

   public async ShowWarning(...messages: string[]): Promise<void> {
      if (!messages || messages.length == 0)
         return;
      // const translatedMessage = await Promise.all(messages.map(async message => await this.translation.getValue(message))); }
      const translatedMessage = messages;
      let messageData = Object.assign(new MessageData, {
         Messages: translatedMessage,
         Type: MessageDataType.Warning
      });
      this.ShowMessage(messageData);
   }

   public async ShowError(messages: string[], details: string): Promise<void> {
      const messageData = Object.assign(new MessageData, {
         Messages: messages,
         Details: details,
         Type: MessageDataType.Error
      });
      this.ShowMessage(messageData);
   }

}
