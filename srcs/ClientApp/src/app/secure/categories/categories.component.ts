import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'fs-categories',
   template: `
      <fs-full-layout>
         <span title>
            {{ 'CATEGORIES_MAIN_TITLE' | translation | async }}
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
export class CategoriesComponent implements OnInit {

   constructor() { }

   ngOnInit() {
   }

}
