import { Component, OnInit } from '@angular/core';
import { EntriesService, EntryFlow } from '../entries.service';

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

}
