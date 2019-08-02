import { Component, OnInit, Inject } from '@angular/core';
import { MatSnackBarRef, MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';
import { MessageData } from './message.models';

@Component({
   selector: 'fs-message',
   templateUrl: './message.component.html',
   styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {

   constructor(
      @Inject(MAT_SNACK_BAR_DATA) public Data: MessageData,
      private snackBar: MatSnackBarRef<MessageComponent>) { }

   ngOnInit() {
   }

   public OnCloseClick() {
      this.snackBar.dismiss();
   }

}
