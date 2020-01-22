import { enAccountType } from '../accounts/accounts.viewmodels';

export class Balance {
   AccountID: number;
   Text: string;
   Type: enAccountType;
   CurrentBalance: number;
   IncomeForecast: number;
   ExpenseForecast: number
}
