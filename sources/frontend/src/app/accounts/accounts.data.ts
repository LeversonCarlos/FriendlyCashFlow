export enum enAccountType { General = 0, Bank = 1, CreditCard = 2, Investment = 3, Service = 4 }

export class AccountEntity {
   AccountID: string;
   Text: string;
   Type: enAccountType;
   ClosingDay?: number;
   DueDay?: number;
   Active: boolean;
}