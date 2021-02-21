import { Component, Input, OnInit } from '@angular/core';
import { EntriesData } from '@elesse/entries';
import { LocalizationService, MessageService } from '@elesse/shared';
import { TransfersData } from '@elesse/transfers';
import { TransactionBase, TransactionEntry, TransactionTransfer } from '../../model/transactions.model';

@Component({
   selector: 'transactions-remove',
   templateUrl: './remove.component.html',
   styleUrls: ['./remove.component.scss']
})
export class RemoveComponent implements OnInit {

   constructor(private entriesData: EntriesData, private transfersData: TransfersData,
      private message: MessageService, private localizationService: LocalizationService) { }

   @Input() Transaction: TransactionBase

   ngOnInit(): void {
   }

   public async OnRemoveClick() {
      if (!this.Transaction)
         return;

      let confirmText = await this.localizationService.GetTranslation("transactions.REMOVE_CONFIRMATION_TEXT");
      confirmText = confirmText.replace('{0}', this.Transaction.Text)

      const confirm = await this.message.Confirm(confirmText, "shared.REMOVE_CONFIRM_COMMAND", "shared.REMOVE_CANCEL_COMMAND");
      if (!confirm)
         return

      if (this.Transaction instanceof TransactionEntry) {
         this.entriesData.RemoveEntry(this.Transaction.Entry);
      }

      if (this.Transaction instanceof TransactionTransfer) {
         this.transfersData.RemoveTransfer(this.Transaction.Transfer);
      }
   }

}
