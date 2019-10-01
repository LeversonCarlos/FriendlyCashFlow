import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';

@Component({
   selector: 'fs-entries-flow',
   templateUrl: './entries-flow.component.html',
   styleUrls: ['./entries-flow.component.scss']
})
export class EntriesFlowComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public ngOnInit() {
   }

   public OnMonthChanged(month: Date) {
      this.service.showFlow(month.getFullYear(), month.getMonth() + 1);
   }

}
