import { Injectable } from '@angular/core';
import { MatSnackBar } from '@elesse/material';
import { MatDialog } from '@elesse/material';
import { LocalizationService } from '../localization/localization.service';
import { ConfirmViewComponent } from './confirm-view/confirm-view.component';
import { MessageViewComponent } from './message-view/message-view.component';
import { ConfirmData, MessageData, MessageType } from './message.models';

@Injectable({
   providedIn: 'root'
})
export class MessageService {

   constructor(private localization: LocalizationService, private snackBar: MatSnackBar, private dialog: MatDialog) { }
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

   public async Confirm(message: string, confirmText = 'SHARED_CONFIRM_COMMAND', cancelText = 'SHARED_CANCEL_COMMAND'): Promise<boolean> {
      let data = Object.assign(new ConfirmData, {});
      if (message)
         data.Message = await this.localization.GetTranslation(message);
         // data.Message = message;
      if (confirmText)
         data.ConfirmText = await this.localization.GetTranslation(confirmText);
         // data.ConfirmText = confirmText;
      if (cancelText)
         data.CancelText = await this.localization.GetTranslation(cancelText);
         // data.CancelText = cancelText;
      const confirmDialog = this.dialog.open(ConfirmViewComponent, {
         panelClass: 'confirm-dialog-panel',
         data: data
      });
      const confirmResult = await confirmDialog.afterClosed().toPromise<boolean>();
      return confirmResult;
   }

}
