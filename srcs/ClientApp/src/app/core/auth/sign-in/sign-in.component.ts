import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BusyService } from 'src/app/shared/busy/busy.service';

@Component({
   selector: 'fs-sign-in',
   templateUrl: './sign-in.component.html',
   styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

   constructor(private busy: BusyService,
      private fb: FormBuilder) { }

   public inputForm: FormGroup;

   public ngOnInit() {
      this.OnFormCreate();
   }

   private OnFormCreate() {
      this.inputForm = this.fb.group({
         UserName: ['', [Validators.required, Validators.email]],
         Password: ['', Validators.required]
      });
   }

   public async OnClick() {
      try {
         this.busy.show();
         console.log(this.inputForm.value);
      }
      catch (ex) { console.error(ex); return null; }
      finally { this.busy.hide(); }
   }

}
