import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { EntryEntity, EntryGroupEntity } from '../entries.data';
import { EntriesService } from '../entries.service';

@Component({
   selector: 'entries-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public get Entries(): Observable<EntryEntity[]> { return this.service.ObserveEntries(); }
   public get EntriesGroups(): Observable<EntryGroupEntity[]> {
      return this.service.ObserveEntries()
         .pipe(
            map(this.ToGroups)
         );
   }

   private ToGroups(entries: EntryEntity[]): EntryGroupEntity[] {
      const groups = entries
         .filter(entry => entry.SearchDate)
         .reduce((acc, cur) => {
            const day = (new Date(cur.SearchDate).getDate()).toString().padStart(2, "0");
            acc[day] = acc[day] || [];
            acc[day].push(cur);
            return acc;
         }, {});
      const days = Object
         .keys(groups)
         .sort((p, n) => p < n ? -1 : 1)
      const result = days
         .map(day => { return { Day: day, Entries: groups[day].sort((p, n) => p.Sorting < n.Sorting ? -1 : 1) }; })
         .map(day => Object.assign(new EntryGroupEntity, { Day: day.Day, Entries: day.Entries, Balance: day.Entries[day.Entries.length - 1].Balance }));
      return result;
   }

   ngOnInit() {
      this.service.RefreshCache();
   }

}
