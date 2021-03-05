import { Component, OnInit } from '@angular/core';
import { AccountsData } from '@elesse/accounts';
import { BalanceData } from '@elesse/balances';
import { CategoriesData, enCategoryType } from '@elesse/categories';
import { EntriesData } from '@elesse/entries';
import { IAccountSelectorService, LocalizationService } from '@elesse/shared';
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

   constructor(
      private accountsData: AccountsData, private categoriesData: CategoriesData,
      private entriesData: EntriesData, private transfersData: TransfersData, private balanceData: BalanceData,
      private accountSelector: IAccountSelectorService,
      private localizationService: LocalizationService) { }

   public TransactionAccounts: Observable<TransactionAccount[]>;
   public get TabIndex(): number { return this.accountSelector.TabIndex; }

   public async ngOnInit(): Promise<void> {
      const transferTo = await this.localizationService.GetTranslation("transactions.TRANSFER_TO_LABEL");
      const transferFrom = await this.localizationService.GetTranslation("transactions.TRANSFER_FROM_LABEL");
      const initialBalanceText = await this.localizationService.GetTranslation("transactions.INITIAL_BALANCE_LABEL");
      const overdueBalanceText = await this.localizationService.GetTranslation("transactions.OVERDUE_BALANCE_LABEL");
      this.TransactionAccounts = TransactionsConverter.Merge(
         this.accountsData.ObserveAccounts(),
         this.categoriesData.ObserveCategories(enCategoryType.Income),
         this.categoriesData.ObserveCategories(enCategoryType.Expense),
         this.entriesData.ObserveEntries, this.transfersData.ObserveTransfers,
         this.balanceData.ObserveBalances,
         transferTo, transferFrom, initialBalanceText, overdueBalanceText)
   }

   public OnTabChanged(tabIndex: number, transactionAccount: TransactionAccount) {
      this.accountSelector.SetTab(tabIndex, transactionAccount.Account.AccountID);
   }

}
