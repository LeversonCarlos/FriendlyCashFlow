import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'shared-empty-list',
   templateUrl: './empty-list.component.html',
   styleUrls: ['./empty-list.component.scss']
})
export class EmptyListComponent implements OnInit {

   constructor() { }

   ngOnInit(): void {
   }

}
