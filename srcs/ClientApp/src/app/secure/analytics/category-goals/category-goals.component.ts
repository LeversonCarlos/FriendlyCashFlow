import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'fs-category-goals',
   templateUrl: './category-goals.component.html',
   styleUrls: ['./category-goals.component.scss']
})
export class CategoryGoalsComponent implements OnInit {

   constructor() { }

   ngOnInit() {
   }

   /*
   private async OnDataLoad() {
      try {
         this.busy.show();
         const result = await this.http.post<boolean>(`api/users/passwordChange`, this.Data).toPromise();
         if (!result) { return; }
         this.auth.signOut();
         this.msg.ShowInfo("SETTINGS_PASSWORD_CHANGE_SUCCESS")
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex) }
      finally { this.busy.hide(); }
   }
   */

}
