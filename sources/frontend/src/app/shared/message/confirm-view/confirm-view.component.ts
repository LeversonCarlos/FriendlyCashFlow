import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmData } from '../message.models';

@Component({
   selector: 'elesse-confirm-view',
   templateUrl: './confirm-view.component.html',
   styleUrls: ['./confirm-view.component.scss']
})
export class ConfirmViewComponent implements OnInit {

   constructor(@Inject(MAT_DIALOG_DATA) public Data: ConfirmData,
      private dialog: MatDialogRef<ConfirmViewComponent>) { }

   ngOnInit(): void {
   }

   OnCancelClick(): void {
      this.dialog.close(false);
   }

   OnConfirmClick(): void {
      this.dialog.close(true);
   }

}
