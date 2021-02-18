import { Component, OnInit } from '@angular/core';
import { AccountsData } from '@elesse/accounts';
import { CategoriesData, enCategoryType } from '@elesse/categories';
import { EntriesData } from '@elesse/entries';
import { TransfersData } from '@elesse/transfers';
import { Observable } from 'rxjs';
import { TransactionsConverter } from '../converter/transactions.converter';
import { TransactionAccount } from '../model/transactions.model';

@Component({
   selector: 'transactions-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private accountsData: AccountsData, private categoriesData: CategoriesData, private entriesData: EntriesData, private transfersData: TransfersData) { }

   public ngOnInit(): void {
      this.TransactionAccounts = TransactionsConverter.Merge(
         this.accountsData.ObserveAccounts(),
         this.categoriesData.ObserveCategories(enCategoryType.Income),
         this.categoriesData.ObserveCategories(enCategoryType.Expense),
         this.entriesData.ObserveEntries, this.transfersData.ObserveTransfers)
   }

   public TransactionAccounts: Observable<TransactionAccount[]>;

}
