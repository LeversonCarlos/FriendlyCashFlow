import { AccountTypeEnum } from ".";

export class AccountModel {
   AccountID: string = '';

   Text: string = '';

   Type: AccountTypeEnum = AccountTypeEnum.General;

   ClosingDay?: number;
   DueDay?: number;
   DueDate?: Date;

   Active: boolean = false;

   public static Parse(value: AccountModel) {
      return Object.assign(new AccountModel, value);
   }
}
