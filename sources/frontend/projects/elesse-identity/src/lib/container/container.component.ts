import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'elesse-identity-container',
   templateUrl: './container.component.html',
   styleUrls: ['./container.component.scss']
})
export class ContainerComponent implements OnInit {

   constructor() { }

   public get AppTitle(): string { return "Friendly Cash Flow"; }

   ngOnInit(): void {
   }

}
