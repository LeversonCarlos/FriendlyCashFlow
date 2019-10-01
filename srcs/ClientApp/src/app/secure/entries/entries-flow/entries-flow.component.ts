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
      console.log('EntriesFlowComponent.OnInit')
   }

   public OnMonthChanged(month: Date) {
      console.log('OnMonthChanged', month)
      this.service.showFlow(month.getFullYear(), month.getMonth() + 1);
   }

}
