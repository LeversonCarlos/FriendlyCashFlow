import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ICommand } from '@interfaces/ICommand';
import { BaseRoute, ViewRoutes } from '@models/accounts';

@Injectable()
export class NavigateCommand {

   constructor(
      private router: Router,
   ) { }

   public async NavigateTo(route: ViewRoutes): Promise<boolean> {
      try {
         await this.router.navigate([`/${BaseRoute}/${route}`]);
         return true;
      }
      catch { return false; }
   }

   public NavigateToHome(): Promise<boolean> {
      return this.router.navigate(['/home']);
   }

}

export interface INavigateCommand extends ICommand<void, boolean> { }
export abstract class INavigateCommand { /* this is required to fake the interface on the compiled JS where there is no interface concept */ }

export const NavigateCommandProvider = { provide: INavigateCommand, useExisting: NavigateCommand };
