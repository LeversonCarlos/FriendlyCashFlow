import { Component, OnInit } from '@angular/core';
import { AccountsState } from '@commands/accounts';

@Component({
   selector: 'app-index-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class IndexListComponent implements OnInit {

   constructor(
      private state: AccountsState,
   ) { }

   public get Accounts() { return this.state.Accounts; }

   ngOnInit(): void {
   }

}
