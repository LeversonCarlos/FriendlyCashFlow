import { Account } from '../accounts/accounts.viewmodels';

export class Transfer {
   TransferID: string
   TransferDate: Date
   TransferValue: number

   IncomeEntryID: number
   IncomeAccountID: number
   IncomeAccountRow: Account
   // IncomeAccountText: string

   ExpenseEntryID: number
   ExpenseAccountID: number
   ExpenseAccountRow: Account
   // ExpenseAccountText: string
}
