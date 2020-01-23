import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../dashboard.service';
import { Entry } from '../../entries/entries.viewmodels';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';

class EntriesVM {
   Text: string
   Entries: Entry[]
}

@Component({
   selector: 'fs-entries',
   templateUrl: './entries.component.html',
   styleUrls: ['./entries.component.scss']
})
export class EntriesComponent implements OnInit {

   constructor(private dashboardService: DashboardService,
      private appInsights: AppInsightsService) { }

   public EntryTypes: EntriesVM[]

   public async ngOnInit() {
      try {
         this.EntryTypes = []
         const entries = await this.dashboardService.getEntries();
         const today = new Date()
         console.log({ entries, today: today })

         let overdueEntries = Object.assign(new EntriesVM, { Text: 'DASHBOARD_ENTRIES_OVERDUE_ENTRIES_TEXT' })
         overdueEntries.Entries = entries.filter(x => x.DueDate < today)
         console.log(overdueEntries)
         if (overdueEntries.Entries && overdueEntries.Entries.length) {
            this.EntryTypes.push(overdueEntries)
         }

         let nextEntries = Object.assign(new EntriesVM, { Text: 'DASHBOARD_ENTRIES_NEXT_ENTRIES_TEXT' })
         nextEntries.Entries = entries.filter(x => x.DueDate >= today)
         if (nextEntries.Entries && nextEntries.Entries.length) {
            this.EntryTypes.push(nextEntries)
         }

      }
      catch (ex) { console.error(ex); this.appInsights.trackException(ex) }
   }

}
