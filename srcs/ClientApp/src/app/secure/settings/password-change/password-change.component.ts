import { Component, OnInit } from '@angular/core';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';

class PasswordChangeVM {
   OldPassword: string;
   NewPassword: string;
   ConfirmPassword: string;
}

@Component({
   selector: 'fs-password-change',
   templateUrl: './password-change.component.html',
   styleUrls: ['./password-change.component.scss']
})
export class PasswordChangeComponent implements OnInit {

   constructor(private busy: BusyService, private appInsights: AppInsightsService,
      private http: HttpClient, private fb: FormBuilder) { }
   private Data: PasswordChangeVM = new PasswordChangeVM()

   public ngOnInit() {
      try {
         this.OnFormCreate();
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex) }
   }

   public inputForm: FormGroup;
   private OnFormCreate() {
      this.inputForm = this.fb.group({
         OldPassword: ['', Validators.required],
         NewPassword: ['', Validators.required],
         ConfirmPassword: ['', Validators.required]
      });
      this.inputForm.valueChanges.subscribe(values => {
         this.Data.OldPassword = values.OldPassword || '';
         this.Data.NewPassword = values.NewPassword || '';
         this.Data.ConfirmPassword = values.ConfirmPassword || '';
      });
   }

   public async OnClick() {
      try {
         if (!this.inputForm.valid) { return; }
         this.busy.show();
         const result = await this.http.post<boolean>(`api/users/passwordChange`, this.Data).toPromise();
         if (!result) { return; }
         console.log('we must logout here')
      }
      catch (ex) { this.appInsights.trackException(ex); console.error(ex) }
      finally { this.busy.hide(); }
   }

}
