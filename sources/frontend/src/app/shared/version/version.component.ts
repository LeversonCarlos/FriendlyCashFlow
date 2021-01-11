import { Component, OnInit } from '@angular/core';
import { version } from 'package.json'

@Component({
   selector: 'elesse-version',
   templateUrl: './version.component.html',
   styleUrls: ['./version.component.scss']
})
export class VersionComponent implements OnInit {

   constructor() { }

   public get Version(): string { return version; }

   ngOnInit(): void {
   }

}
