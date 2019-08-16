import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { User } from '../auth.models';

@Component({
   selector: 'fs-activate',
   templateUrl: './activate.component.html',
   styleUrls: ['./activate.component.scss']
})
export class ActivateComponent implements OnInit {

   constructor(private service: AuthService, private activatedRoute: ActivatedRoute, private router: Router) { }

   public activatedUser: User;

   public async ngOnInit() {
      const userID: string = this.activatedRoute.snapshot.params["id"];
      const activationCode: string = this.activatedRoute.snapshot.params["code"];
      await this.service.activateUser(userID, activationCode);
      this.router.navigate(['/signin']);
   }

}
