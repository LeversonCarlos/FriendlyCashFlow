import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'fs-categories',
   templateUrl: './categories.component.html',
   styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {

   constructor() { }

   public selectedValue: string;

   public filteredOptions: string[] = ['Casa', "Camelo", 'Coisas']

   ngOnInit() {
   }

}
