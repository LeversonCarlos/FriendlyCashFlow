import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmData } from './message.models';

@Component({
   selector: 'fs-confirm',
   templateUrl: './confirm.component.html',
   styleUrls: ['./confirm.component.scss']
})
export class ConfirmComponent implements OnInit {

   constructor(
      @Inject(MAT_DIALOG_DATA) public Data: ConfirmData,
      private dialog: MatDialogRef<ConfirmComponent>
   ) { }

   ngOnInit() {
   }

   OnCancelClick(): void {
      this.dialog.close(false);
   }

   OnConfirmClick(): void {
      this.dialog.close(true);
   }

}
