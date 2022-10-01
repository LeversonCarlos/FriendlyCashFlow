import { Injectable } from '@angular/core';
import { ICommand } from '@interfaces/ICommand';
import { ShowAccountsCommand } from '.';

@Injectable()
export class InitializeCommand implements ICommand<void, boolean> {

   constructor(
      private showAccounts: ShowAccountsCommand,
   ) { }

   public Handle(): Promise<boolean> {
      console.log('InitializeCommand');
      return this.showAccounts.Handle();
   }

}
