import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
   selector: 'fs-app-version',
   templateUrl: './app-version.component.html',
   styleUrls: ['./app-version.component.scss']
})
export class AppVersionComponent implements OnInit {

   constructor() { }

   public appVersion: string = environment.appVersion

   ngOnInit() {
   }

}
