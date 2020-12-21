import { Component } from '@angular/core';
import { InsightsService } from 'elesse-shared';

@Component({
   selector: 'app-root',
   template: `
      <div class="content">
         <h1>Welcome to {{title}}!</h1>
         <span style="display: block">{{ title }} app is running!</span>
         <a routerLink="/identity/register">Register</a>
         <a routerLink="/identity/login">Login</a>
         <a routerLink="/identity/logout">Logout</a>
         <a mat-raised-button routerLink="/identity/change-password">Change Password</a>
      </div>
      <router-outlet></router-outlet>
      <shared-busy></shared-busy>
   `,
   styles: [`
      .content {
         height:80vh;
         display:flex;
         flex-direction:column;
         justify-content:center;
         align-items:center;
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
