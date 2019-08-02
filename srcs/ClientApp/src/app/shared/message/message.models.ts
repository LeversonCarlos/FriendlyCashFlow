export enum MessageDataType {
   Information = 0, Warning = 1, Error = 2
}

export class MessageData {
   // Title: string;
   Messages: string[];
   Details: string;
   Type: MessageDataType;
   Duration: number;
}

export class ConfirmData {
   // Title: string;
   Message: string;
   CancelText: string;
   ConfirmText: string;
 }
