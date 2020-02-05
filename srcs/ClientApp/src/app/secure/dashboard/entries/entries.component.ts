import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../dashboard.service';
import { Entry } from '../../entries/entries.viewmodels';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';
import { BehaviorSubject } from 'rxjs';

class EntriesVM {
   Text: string
   Overdue: boolean
   Entries: Entry[]
}

@Component({
   selector: 'fs-entries',
   templateUrl: './entries.component.html',
   styleUrls: ['./entries.component.scss']
})
export class EntriesComponent implements OnInit {

   constructor(private dashboardService: DashboardService, private appInsights: AppInsightsService) { }

   private Key: string = 'DASHBOARD_ENTRIES_DATA'
   public Data = new BehaviorSubject<EntriesVM[]>([])

   public async ngOnInit() {
      this.cacheLoad()
      const data = await this.getRefreshedData()
      this.cacheSave(data)
   }

   private cacheLoad() {
      try {
         const jsonData = localStorage.getItem(this.Key)
         if (jsonData && jsonData != '') {
            const data: EntriesVM[] = JSON.parse(jsonData)
            if (data) {
               this.Data.next(data)
            }
         }
      }
      catch (ex) { this.appInsights.trackException(ex) }
   }

   private async getRefreshedData(): Promise<EntriesVM[]> {
      try {
         let entryTypes = []
         const entries = await this.dashboardService.getEntries();
         const today: any = (new Date()).toISOString()

         let overdueEntries = Object.assign(new EntriesVM, { Text: 'DASHBOARD_ENTRIES_OVERDUE_ENTRIES_TEXT', Overdue: true })
         overdueEntries.Entries = entries.filter(x => x.DueDate < today)
         if (overdueEntries.Entries && overdueEntries.Entries.length) {
            entryTypes.push(overdueEntries)
         }

         let nextEntries = Object.assign(new EntriesVM, { Text: 'DASHBOARD_ENTRIES_NEXT_ENTRIES_TEXT', Overdue: false })
         nextEntries.Entries = entries.filter(x => x.DueDate >= today)
         if (nextEntries.Entries && nextEntries.Entries.length) {
            entryTypes.push(nextEntries)
         }

         return entryTypes;
      }
      catch (ex) { this.appInsights.trackException(ex); return null; }
   }

   private async cacheSave(data: EntriesVM[]) {
      try {
         this.Data.next(data)
         const jsonData = JSON.stringify(data);
         localStorage.setItem(this.Key, jsonData)
      }
      catch (ex) { this.appInsights.trackException(ex) }
   }

   public GetUrl(entry: Entry): string {
      return `/entries/entry/${entry.EntryID}`
   }

}
