import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../identity.service';

@Component({
   selector: 'identity-logout',
   templateUrl: './logout.component.html',
   styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

   constructor(private service: IdentityService) { }

   ngOnInit(): void {
      this.service.Logout();
   }

}
