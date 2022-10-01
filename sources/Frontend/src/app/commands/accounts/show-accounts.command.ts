import { Injectable } from '@angular/core';
import { ICommand } from '@interfaces/ICommand';
import { ViewRoutes } from '@models/accounts';
import { SearchRepository } from '@repositories/accounts';
import { AccountsState, NavigateCommand } from '.';

@Injectable()
export class ShowAccountsCommand implements ICommand<void, boolean> {

   constructor(
      private state: AccountsState,
      private search: SearchRepository,
      private navigate: NavigateCommand,
   ) {
      this.state.SearchTermsSubject.subscribe(searchTerms => this.RefreshData());
   }

   public async Handle(): Promise<boolean> {
      this.RefreshData();
      if (!await this.navigate.Handle(ViewRoutes.Index))
         return false;
      return true;
   }

   private RefreshData() {
      const searchTerms = this.state.SearchTerms;
      this.search
         .Handle(searchTerms)
         .then(result => this.state.Accounts = result);
   }

}
