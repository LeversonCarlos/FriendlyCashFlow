import { Component } from '@angular/core';
import { InsightsService } from '@elesse/shared';

@Component({
   selector: 'elesse-cash-flow-root',
   template: `
      <div class="content">
         <h1>Welcome to {{title}}!</h1>
         <h3 style="display: block">{{ title }} app is running!</h3>
         <div class="menu">
            <a routerLink="/identity/register">Register</a>
            <a routerLink="/identity/login">Login</a>
            <a routerLink="/identity/logout">Logout</a>
            <a routerLink="/identity/change-password">Change Password</a>
         </div>
         <router-outlet></router-outlet>
      </div>
      <shared-busy></shared-busy>
   `,
   styles: [`
      .content {
         height:100%;
         display:flex;
         flex-direction:column;
         justify-content:center;
         align-items:center;
      }
      .menu {
         display:flex;
         width:50%;
         justify-content:space-evenly;
      }
      h1 {
         text-transform:uppercase;
      }
   `]
})
export class AppComponent {
   title = 'Cash Flow';

   constructor(private insights: InsightsService) {
      this.insights.TrackEvent('Application Opened');
   }

}
