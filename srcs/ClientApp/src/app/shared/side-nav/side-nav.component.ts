import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/auth/auth.service';

@Component({
   selector: 'fs-side-nav',
   templateUrl: './side-nav.component.html',
   styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit {

   constructor(private auth: AuthService) { }

   ngOnInit() {
   }

   public OnLogoutClick() {
      this.auth.signOut();
   }

}
