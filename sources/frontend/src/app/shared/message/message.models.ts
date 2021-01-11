export enum MessageType {
   Information = 0, Warning = 1, Error = 2
}

export class MessageData {
   Messages: string[];
   Details: string;
   Type: MessageType;
   get Duration(): number {
      switch (this.Type) {
         case MessageType.Error:
            return 0;
         case MessageType.Warning:
            return 5000;
         default:
            return 3000;
      }
   }
}

export class ConfirmData {
   Message: string;
   CancelText: string;
   ConfirmText: string;
}
