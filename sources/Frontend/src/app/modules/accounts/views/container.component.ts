import { Component, OnInit } from '@angular/core';
import { IInitializeCommand } from '@commands/accounts';

@Component({
   selector: 'app-container',
   template: `
    <p>
      container works!
    </p>
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
