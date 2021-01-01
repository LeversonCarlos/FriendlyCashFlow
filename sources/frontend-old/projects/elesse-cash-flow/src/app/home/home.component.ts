import { Component, OnInit } from '@angular/core';
import { TokenService } from '@elesse/shared';

@Component({
   selector: 'elesse-cash-flow-home',
   templateUrl: './home.component.html',
   styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

   constructor(private tokenService: TokenService) { }

   public get IsAuthenticated(): boolean { return this.tokenService && this.tokenService.HasToken; }

   ngOnInit(): void {
   }

}
