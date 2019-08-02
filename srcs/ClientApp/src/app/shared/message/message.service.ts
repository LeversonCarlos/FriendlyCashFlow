import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MessageData, MessageDataType } from './message.models';
import { MessageComponent } from './message.component';
import { TranslationService } from '../translation/translation.service';

@Injectable({
   providedIn: 'root'
})
export class MessageService {

   constructor(private snackBar: MatSnackBar, private translation: TranslationService) { }

   private ShowMessage(data: MessageData): void {
      this.snackBar.openFromComponent(MessageComponent, {
         panelClass: 'message-panel',
         data: data,
         duration: data.Duration,
         horizontalPosition: 'right',
         verticalPosition: 'top'
      });
   }

   public async ShowInfo(...messages: string[]): Promise<void> {
      let data = Object.assign(new MessageData, {
         Type: MessageDataType.Information,
         Duration: 3000
      });
      if (messages) { data.Messages = await Promise.all(messages.map(async message => await this.translation.getValue(message))); }
      this.ShowMessage(data);
   }

   public async ShowWarning(...messages: string[]): Promise<void> {
      let data = Object.assign(new MessageData, {
         Type: MessageDataType.Warning,
         Duration: 5000
      });
      if (messages) { data.Messages = await Promise.all(messages.map(async message => await this.translation.getValue(message))); }
      this.ShowMessage(data);
   }

   public async ShowError(messages: string[], details: string): Promise<void> {
      let data = Object.assign(new MessageData, {
         Messages: messages,
         Details: details,
         Type: MessageDataType.Error,
         Duration: 0
      });
      this.ShowMessage(data);
   }

}
