import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ICommand } from '@interfaces/ICommand';
import { FrontendRoute, ViewRoutes } from '@models/accounts';

@Injectable()
export class NavigateCommand implements ICommand<ViewRoutes, boolean> {

   constructor(
      private router: Router,
   ) { }

   public async Handle(route: ViewRoutes): Promise<boolean> {
      try {
         await this.router.navigate([`/${FrontendRoute}/${route}`]);
         return true;
      }
      catch { return false; }
   }

   public NavigateToHome(): Promise<boolean> {
      return this.router.navigate(['/home']);
   }

}
