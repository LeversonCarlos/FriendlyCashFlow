import { Component, OnInit } from '@angular/core';
import { IInitializeCommand } from '@commands/accounts';

@Component({
   selector: 'app-container',
   template: `
      <router-outlet></router-outlet>
   `,
   styles: [
   ]
})
export class ContainerComponent implements OnInit {

   constructor(
      initialize: IInitializeCommand
   ) {
      initialize.Handle();
   }

   ngOnInit(): void {
   }

}
