import { Component, OnInit } from '@angular/core';
import { AccountsData } from '@elesse/accounts';
import { EntriesData } from '@elesse/entries';
import { Observable } from 'rxjs';
import { TransactionsConverter } from '../converter/transactions.converter';
import { TransactionAccount } from '../model/transactions.model';

@Component({
   selector: 'transactions-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private accountsData: AccountsData, private entriesData: EntriesData) { }

   public ngOnInit(): void {
      this.TransactionAccounts = TransactionsConverter.Merge(this.accountsData.ObserveAccounts(), this.entriesData.ObserveEntries())
   }

   public TransactionAccounts: Observable<TransactionAccount[]>;

}
