import { Component, OnInit } from '@angular/core';
import { InitializeCommand } from '@commands/accounts';

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
      initialize: InitializeCommand
   ) {
      initialize.Handle();
   }

   ngOnInit(): void {
   }

}
