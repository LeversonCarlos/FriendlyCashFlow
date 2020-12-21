import { Component, OnInit } from '@angular/core';
import { BusyService } from './busy.service';

@Component({
   selector: 'shared-busy',
   templateUrl: './busy.component.html',
   styleUrls: ['./busy.component.css']
})
export class BusyComponent implements OnInit {

   constructor(private service: BusyService) { }

   public get IsBusy(): boolean { return this.service.IsBusy; }

   ngOnInit(): void {
   }

}
