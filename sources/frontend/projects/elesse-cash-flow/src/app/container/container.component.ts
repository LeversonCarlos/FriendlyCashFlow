import { Component, OnInit } from '@angular/core';
import { TokenService } from '@elesse/shared';

@Component({
   selector: 'elesse-cash-flow-container',
   templateUrl: './container.component.html',
   styleUrls: ['./container.component.scss']
})
export class ContainerComponent implements OnInit {

   constructor(private tokenService: TokenService) { }

   public get IsAuthenticated(): boolean { return this.tokenService && this.tokenService.HasToken; }

   ngOnInit(): void {
   }

}
