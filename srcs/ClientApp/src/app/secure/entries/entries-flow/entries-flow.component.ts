import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';
import { EntryFlow, Entry } from '../entries.viewmodels';
import { ActivatedRoute } from '@angular/router';
import { MediaMatcher } from '@angular/cdk/layout';

@Component({
   selector: 'fs-entries-flow',
   templateUrl: './entries-flow.component.html',
   styleUrls: ['./entries-flow.component.scss']
})
export class EntriesFlowComponent implements OnInit {

   constructor(private service: EntriesService, private route: ActivatedRoute, private media: MediaMatcher) {
      this.mobileQuery = this.media.matchMedia('(max-width: 960px)');
   }

   public mobileQuery: MediaQueryList
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

         const currentAccount = account;
         const currentMonth = new Date(`${year.toString().padStart(4, "20")}-${(month + 0).toString().padStart(2, "0")}-01 12:00:00`);
         this.service.setCurrentData(currentMonth, currentAccount);

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
