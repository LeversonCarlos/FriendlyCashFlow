import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'identity-ElesseIdentity',
   template: `
    <p>
      elesse-identity works!
    </p>
    <router-outlet></router-outlet>
   `,
   styles: [
   ]
})
export class ElesseIdentityComponent implements OnInit {

   constructor() { }

   ngOnInit(): void {
   }

}
