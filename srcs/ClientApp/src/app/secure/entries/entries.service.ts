import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

export class Entry {
   EntryID: number;
   Type: any;
   Text: string;
   PatternID: number;
   CategoryID: number;

   DueDate: Date;
   EntryValue: number;

   Paid: boolean;
   PayDate?: Date;
   AccountID?: number;

   RecurrencyID?: number;
   Recurrency?: any;
}

@Injectable({
   providedIn: 'root'
})
export class EntriesService {

   constructor(private busy: BusyService,
      private http: HttpClient, private router: Router) { }

   // NAVIGATES
   public showFlow(year: number, month: number) { this.router.navigate(['/entries', 'flow', year, month]); }
   public showSearch(searchText: string, accountID: number = 0) { this.router.navigate(['/entries', 'search', searchText, accountID]); }
   public showEntryDetails(id: number) { this.router.navigate(['/entries', 'entry', id], { skipLocationChange: true }); }
   public showTransferDetails(id: number) { this.router.navigate(['/entries', 'transfer', id], { skipLocationChange: true }); }
   public showEntryNew() { this.router.navigate(['/entries', 'entry', 'new'], { skipLocationChange: true }); }
   public showTransferNew() { this.router.navigate(['/entries', 'transfer', 'new'], { skipLocationChange: true }); }

   // ENTRIES
   public async getEntries(year: number, month: number, accountID: number = 0): Promise<Entry[]> {
      try {
         this.busy.show();
         let url = `api/entries/flow/${year}/${month}`;
         if (accountID && accountID > 0) { url = `${url}/${accountID}`; }
         const dataList = await this.http.get<Entry[]>(url)
            .pipe(map(items => items.map(item => Object.assign(new Entry, item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

}
