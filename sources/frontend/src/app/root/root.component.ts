import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'elesse-root',
   templateUrl: './root.component.html',
   styleUrls: ['./root.component.scss']
})
export class RootComponent implements OnInit {

   constructor() { }

   title = 'CashFlow';

   ngOnInit(): void {
   }

}
