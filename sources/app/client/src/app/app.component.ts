import { Component } from '@angular/core';

@Component({
   selector: 'app-root',
   template: `
      <div class="content">
         <h1>Welcome to {{title}}!</h1>
         <span style="display: block">{{ title }} app is running!</span>
         <identity-ElesseIdentity></identity-ElesseIdentity>
      </div>
      <router-outlet></router-outlet>
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
   title = 'FriendlyCashFlow';
}
