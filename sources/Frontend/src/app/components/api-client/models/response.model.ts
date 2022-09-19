import { MessageModel } from '@components/messages'

export class BaseResponse {
   OK: boolean = false;
   Messages: MessageModel[] = [];

   static CreateError<T>(err: any): T {
      const result: any = {
         OK: false,
         Messages: [MessageModel.CreateError(err)]
      };
      return result;
   }
}


