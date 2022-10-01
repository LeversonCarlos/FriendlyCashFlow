import { Injectable } from '@angular/core';
import { ICommand } from '@interfaces/ICommand';
import { ViewRoutes } from '@models/accounts';
import { SearchRepository } from '@repositories/accounts';
import { AccountsState, NavigateCommand } from '.';

@Injectable()
export class ShowAccountsCommand implements IShowAccountsCommand {

   constructor(
      private state: AccountsState,
      private search: SearchRepository,
      private navigate: NavigateCommand,
   ) {
      this.state.SearchTermsSubject.subscribe(searchTerms => this.RefreshData());
   }

   public async Handle(): Promise<boolean> {
      this.RefreshData();
      if (!await this.navigate.NavigateTo(ViewRoutes.Index))
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

export interface IShowAccountsCommand extends ICommand<void, boolean> { }
export abstract class IShowAccountsCommand { /* this is required to fake the interface on the compiled JS where there is no interface concept */ }

export const ShowAccountsCommandProvider = { provide: IShowAccountsCommand, useExisting: ShowAccountsCommand };
