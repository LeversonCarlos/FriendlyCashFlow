import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'elesse-anonymous-container',
   templateUrl: './anonymous-container.component.html',
   styleUrls: ['./anonymous-container.component.scss']
})
export class AnonymousContainerComponent implements OnInit {

   constructor() { }

   public get AppTitle(): string { return "Friendly Cash Flow"; }

   ngOnInit(): void {
   }

}
