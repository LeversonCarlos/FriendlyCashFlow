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
   ) { }

   public async Handle(): Promise<boolean> {
      this.state.SearchTermsSubject.subscribe(searchTerms => this.RefreshData());
      // this.RefreshData();
      if (!await this.navigate.Handle(ViewRoutes.Index))
         return false;
      return true;
   }

   private async RefreshData() {
      const searchTerms = this.state.SearchTerms;
      this.state.Accounts = await this.search.Handle(searchTerms);
   }

}
