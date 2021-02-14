import { AccountEntity } from "@elesse/accounts";
import { EntryEntity } from "@elesse/entries";
import { Observable } from "rxjs";
import { TransactionAccount } from "../model/transactions.model";

export class TransactionsParser {

   public static Parse(accountsParam: AccountEntity[], entriesParam: EntryEntity[]): TransactionAccount[] {
      if (accountsParam == null || accountsParam.length == 0)
         return [];

      return null;
   }

}
