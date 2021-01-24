import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { EntryEntity } from '../entries.data';
import { EntriesService } from '../entries.service';

@Component({
   selector: 'entries-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public get Entries(): Observable<EntryEntity[]> { return this.service.ObserveEntries(); }

   ngOnInit(): void {
      this.service.RefreshCache();
   }

}
