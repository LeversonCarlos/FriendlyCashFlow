import { Component, Inject, OnInit } from '@angular/core';
import { MatSnackBarRef, MAT_SNACK_BAR_DATA } from '@elesse/material';
import { MessageData } from '../message.models';

@Component({
   selector: 'elesse-message-view',
   templateUrl: './message-view.component.html',
   styleUrls: ['./message-view.component.scss']
})
export class MessageViewComponent implements OnInit {

   constructor(@Inject(MAT_SNACK_BAR_DATA) public Data: MessageData,
      private snackBar: MatSnackBarRef<MessageViewComponent>) { }

   ngOnInit(): void {
   }

   public OnCloseClick() {
      this.snackBar.dismiss();
   }

}
