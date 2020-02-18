import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'fs-entries',
   template: `
      <router-outlet></router-outlet>
   `,
   styles: []
})
export class EntriesComponent implements OnInit {

   constructor() { }

   ngOnInit() {
   }

}
