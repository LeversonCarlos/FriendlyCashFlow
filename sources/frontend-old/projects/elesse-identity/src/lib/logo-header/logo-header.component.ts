import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'elesse-identity-logo-header',
   templateUrl: './logo-header.component.html',
   styleUrls: ['./logo-header.component.scss']
})
export class LogoHeaderComponent implements OnInit {

   constructor() { }

   public get AppTitle(): string { return "Friendly Cash Flow"; }

   ngOnInit(): void {
   }

}
