import { Component, OnInit } from '@angular/core';
import { EntriesService, Entry } from '../entries.service';

@Component({
   selector: 'fs-entries-flow',
   templateUrl: './entries-flow.component.html',
   styleUrls: ['./entries-flow.component.scss']
})
export class EntriesFlowComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public get DataList(): Entry[] { return this.service.DataList }

   public ngOnInit() {
   }

}
