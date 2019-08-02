import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MessageData, MessageDataType, ConfirmData } from './message.models';
import { MessageComponent } from './message.component';
import { TranslationService } from '../translation/translation.service';
import { ConfirmComponent } from './confirm.component';
import { MatDialog } from '@angular/material/dialog';

@Injectable({
   providedIn: 'root'
})
export class MessageService {

   constructor(private translation: TranslationService,
      private snackBar: MatSnackBar, private dialog: MatDialog, ) { }

   private ShowMessage(data: MessageData): void {
      this.snackBar.openFromComponent(MessageComponent, {
         panelClass: 'message-snack-panel',
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

   public async Confirm(message: string, confirmText = 'BASE_COMMAND_CONFIRM', cancelText = 'BASE_COMMAND_CANCEL'): Promise<boolean> {
      try {
         let data = Object.assign(new ConfirmData, {});
         if (message) { data.Message = await this.translation.getValue(message); }
         if (confirmText) { data.ConfirmText = await this.translation.getValue(confirmText); }
         if (cancelText) { data.CancelText = await this.translation.getValue(cancelText); }
         const confirmDialog = this.dialog.open(ConfirmComponent, {
            panelClass: 'confirm-dialog-panel',
            data: data
         });
         const confirmResult = await confirmDialog.afterClosed().toPromise<boolean>();
         return confirmResult;
      }
      catch (ex) { console.error(ex); }
   }

}
