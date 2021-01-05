import { Component, OnInit } from '@angular/core';
import { TokenService } from '@elesse/identity';

@Component({
   selector: 'elesse-home',
   template: `
      <elesse-anonymous-home *ngIf="!IsAuthenticated"></elesse-anonymous-home>
      <elesse-authenticated-home *ngIf="IsAuthenticated"></elesse-authenticated-home>
   `,
   styles: []
})
export class HomeComponent implements OnInit {

   constructor(private tokenService: TokenService) { }

   public get IsAuthenticated(): boolean { return this.tokenService && this.tokenService.HasToken; }

   ngOnInit(): void {
   }

}
