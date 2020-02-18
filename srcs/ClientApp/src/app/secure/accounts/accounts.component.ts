import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'fs-accounts',
   template: `
      <fs-full-layout>
         <span title>
            {{ 'ACCOUNTS_MAIN_TITLE' | translation | async }}
         </span>
         <div toolbar>
         </div>
         <div content class="full-screen">
            <router-outlet></router-outlet>
         </div>
      </fs-full-layout>
   `,
   styles: []
})
export class AccountsComponent implements OnInit {

   constructor() { }

   ngOnInit() {
   }

}
