import { Component, OnInit, Input } from '@angular/core';

@Component({
   selector: 'fab-button',
   templateUrl: './fab-button.component.html',
   styleUrls: ['./fab-button.component.scss']
})
export class FabButtonComponent implements OnInit {

   constructor() { this.icon = "add"; }

   @Input()
   public icon: string;

   ngOnInit() {
   }

}
