import { Injectable } from '@angular/core';
import { ICommand } from '@interfaces/ICommand';
import { IShowAccountsCommand } from '.';

@Injectable()
export class InitializeCommand implements IInitializeCommand {

   constructor(
      private showAccounts: IShowAccountsCommand,
   ) { }

   public Handle(): Promise<boolean> {
      console.log('InitializeCommand');
      return this.showAccounts.Handle();
   }

}

export interface IInitializeCommand extends ICommand<void, boolean> { }
export abstract class IInitializeCommand { /* this is required to fake the interface on the compiled JS where there is no interface concept */ }

export const InitializeCommandProvider = { provide: IInitializeCommand, useExisting: InitializeCommand };
