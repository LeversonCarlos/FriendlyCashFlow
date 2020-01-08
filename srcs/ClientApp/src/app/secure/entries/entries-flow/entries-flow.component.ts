import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';
import { EntryFlow, Entry } from '../entries.viewmodels';

@Component({
   selector: 'fs-entries-flow',
   templateUrl: './entries-flow.component.html',
   styleUrls: ['./entries-flow.component.scss']
})
export class EntriesFlowComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public get FlowList(): EntryFlow[] { return this.service.FlowList }

   public ngOnInit() {
   }

   public OnItemClick(item: Entry) {
      if (item.TransferID && item.TransferID != '') {
         this.service.showTransferDetails(item.EntryID)
      }
      else {
         this.service.showEntryDetails(item.EntryID)
      }
   }

}
