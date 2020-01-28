export enum MessageDataType {
   Information = 0, Warning = 1, Error = 2
}

export class MessageData {
   // Title: string;
   Messages: string[];
   Details: string;
   Type: MessageDataType;
   get Duration(): number {
      switch (this.Type) {
         case MessageDataType.Error:
            return 0;
         case MessageDataType.Warning:
            return 5000;
         default:
            return 3000;
      }
   }
}

export class ConfirmData {
   // Title: string;
   Message: string;
   CancelText: string;
   ConfirmText: string;
}
