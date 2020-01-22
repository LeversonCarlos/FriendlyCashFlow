import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { environment } from 'src/environments/environment';

@Component({
   selector: 'fs-home',
   templateUrl: './home.component.html',
   styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

   constructor(public auth: AuthService) { }

   public appVersion: string = environment.appVersion

   public async ngOnInit() {
   }

}
