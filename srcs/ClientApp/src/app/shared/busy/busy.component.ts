import { Component, OnInit } from '@angular/core';
import { BusyService } from './busy.service';

@Component({
   selector: 'fs-busy',
   templateUrl: './busy.component.html',
   styleUrls: ['./busy.component.scss']
})
export class BusyComponent implements OnInit {

   constructor(private service: BusyService) { }
   public get IsBusy(): boolean { return this.service.IsBusy; }

   ngOnInit() {
   }

}
