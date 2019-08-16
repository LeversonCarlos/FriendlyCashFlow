import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { User } from '../auth.models';

@Component({
   selector: 'fs-sign-up',
   templateUrl: './sign-up.component.html',
   styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {

   constructor(private busy: BusyService,
      private http: HttpClient, private fb: FormBuilder) { }

   public inputForm: FormGroup;

   public ngOnInit() {
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         UserText: ['', Validators.required],
         UserMail: ['', Validators.required],
         Password: ['', Validators.required]
      });
   }

   public async OnClick() {
      try {
         this.busy.show();
         const result = await this.http.post<User>(`api/users`, this.inputForm.value).toPromise();;
         return result != null && result.UserID != null;
      }
      catch (ex) { console.error(ex); return null; }
      finally { this.busy.hide(); }
   }

}
