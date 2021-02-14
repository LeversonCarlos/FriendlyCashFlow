import { AccountEntity } from "@elesse/accounts";
import { EntryEntity } from "@elesse/entries";
import { TransactionAccount } from "../model/transactions.model";

export class TransactionsConverter {

   public static Convert(accountsParam: AccountEntity[], entriesParam: EntryEntity[]): TransactionAccount[] {
      if (accountsParam == null || accountsParam.length == 0)
         return [];

      let accountsResult: TransactionAccount[] = [];
      for (let accountIndex = 0; accountIndex < accountsParam.length; accountIndex++) {
         const account = accountsParam[accountIndex];
         const accountResult = TransactionAccount.Parse(account);
         accountsResult.push(accountResult);
      }

      return accountsResult;
   }

}
