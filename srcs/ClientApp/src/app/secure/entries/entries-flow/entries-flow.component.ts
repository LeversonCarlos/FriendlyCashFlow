import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';
import { EntryFlow, Entry } from '../entries.viewmodels';
import { ActivatedRoute } from '@angular/router';

@Component({
   selector: 'fs-entries-flow',
   templateUrl: './entries-flow.component.html',
   styleUrls: ['./entries-flow.component.scss']
})
export class EntriesFlowComponent implements OnInit {

   constructor(private service: EntriesService, private route: ActivatedRoute) { }
   public get FlowList(): EntryFlow[] { return this.service.FlowList }


   /* INIT */
   public ngOnInit() {
      try {

         let year: number = Number(this.route.snapshot.params.year);
         let month: number = Number(this.route.snapshot.params.month);
         let account: number = Number(this.route.snapshot.params.account);

         // DEFAULT YEAR/MONTH
         if (
            (!year || isNaN(year) || year < 1901 || year > 3000) ||
            (!month || isNaN(month) || month < 1 || month > 12)
         ) {
            const date = new Date();
            year = date.getFullYear();
            month = date.getMonth() + 1;
            this.service.showFlow(year, month);
            return;
         }

         this.CurrentAccount = account;
         this.CurrentMonth = new Date(`${year.toString().padStart(4, "20")}-${(month + 0).toString().padStart(2, "0")}-01 12:00:00`);

      }
      catch (ex) { console.error(ex); }
   }


   /* CURRENT ACCOUNT */
   public get CurrentAccount(): number {
      return this.service.CurrentData.CurrentAccount || 0;
   }
   public set CurrentAccount(val: number) {
      this.setData(this.CurrentMonth, val)
   }


   /* CURRENT MONTH */
   public get CurrentMonth(): Date {
      return this.service.CurrentData.CurrentMonth;
   }
   public set CurrentMonth(val: Date) {
      this.setData(val, this.CurrentAccount)
   }


   /* SET DATA */
   private async setData(currentMonth: Date, currentAccount: number) {
      try {
         this.service.CurrentData.setFlow(currentMonth, currentAccount);

         const year = this.CurrentMonth.getFullYear()
         const month = this.CurrentMonth.getMonth() + 1
         const account = this.CurrentAccount

         this.service.loadFlowList(year, month, account)
         this.service.showCurrentList()
      }
      catch (ex) { console.error(ex); }
   }


   /* ITEM CLICK */
   public OnItemClick(item: Entry) {
      if (item.TransferID && item.TransferID != '') {
         this.service.showTransferDetails(item.TransferID)
      }
      else {
         this.service.showEntryDetails(item.EntryID)
      }
   }

}
