import { Component, OnInit } from '@angular/core';
import { BusyService } from './busy.service';

@Component({
   selector: 'fs-busy',
   template: `<mat-progress-bar *ngIf="IsBusy" mode="indeterminate"></mat-progress-bar>`,
   styles: [`
      .mat-progress-bar {
         position: fixed;
         top:0; left:0; right:0; height:2px;
         z-index: 1001;
      }
   `]
})
export class BusyComponent implements OnInit {

   constructor(private service: BusyService) { }
   public get IsBusy(): boolean { return this.service.IsBusy; }

   ngOnInit() {
   }

}
