import { MessageTypeEnum } from "..";

export class MessageModel {
   Text: string = '';
   Type: MessageTypeEnum = MessageTypeEnum.Message;

   public static Parse(data: MessageModel): MessageModel {
      return Object.assign(new MessageModel, data);
   }

   public static CreateWarning(text: string) {
      return MessageModel.Create(text, MessageTypeEnum.Warning);
   }

   public static CreateError(text: any) {
      return MessageModel.Create(text, MessageTypeEnum.Error);
   }

   private static Create(text: string, type: MessageTypeEnum) {
      const result: MessageModel = {
         Text: text,
         Type: type
      };
      return result;
   }

}
