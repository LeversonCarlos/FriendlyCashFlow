import { EnumVM } from 'src/app/shared/common/common.models';

export enum enAccountType { General = 0, Bank = 1, CreditCard = 2, Investment = 3, Service = 4 };
export class AccountType extends EnumVM<enAccountType> {
   get Icon(): string {
      switch (this.Value) {
         case enAccountType.Bank:
            return 'account_balance';
         case enAccountType.CreditCard:
            return 'credit_card';
         case enAccountType.Investment:
            return 'local_atm';
         case enAccountType.Service:
            return 'card_giftcard';
         default:
            return 'account_balance_wallet';
      }
   }
}

export class Account {
   AccountID: number;
   Text: string;
   Type: enAccountType;
   ClosingDay?: number;
   DueDay?: number;
   DueDate?: Date;
   Active: boolean;
   get Icon(): string {
      switch (this.Type) {
         case enAccountType.Bank:
            return 'account_balance';
         case enAccountType.CreditCard:
            return 'credit_card';
         case enAccountType.Investment:
            return 'local_atm';
         case enAccountType.Service:
            return 'card_giftcard';
         default:
            return 'account_balance_wallet';
      }
   }
}
