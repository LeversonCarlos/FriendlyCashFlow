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
   CategoryText: string;

   DueDate: Date;
   EntryValue: number;

   Paid: boolean;
   PayDate?: Date;
   AccountID?: number;
   AccountText: string;

   RecurrencyID?: number;
   Recurrency?: any;

   TransferID: string;

   BalanceTotalValue: number
   BalancePaidValue: number
   Sorting: number
}

export class EntryFlow {
   Day: string
   EntryList: Entry[]
   BalanceTotalValue: number
   BalancePaidValue: number
}

@Injectable({
   providedIn: 'root'
})
export class EntriesService {

   constructor(private busy: BusyService,
      private http: HttpClient, private router: Router) { }

   // NAVIGATES
   public showFlow(year?: number, month?: number) {
      console.log('showFlowA', { year, month })
      if (!year && this.CurrentMonth) { year = this.CurrentMonth.getFullYear(); }
      if (!month && this.CurrentMonth) { month = this.CurrentMonth.getMonth() + 1; }
      console.log('showFlowB', { year, month })
      this.router.navigate(['/entries', 'flow', year, month]);
   }
   public showSearch(searchText: string, accountID: number = 0) { this.router.navigate(['/entries', 'search', searchText, accountID]); }
   public showEntryDetails(id: number) { this.router.navigate(['/entries', 'entry', id], { skipLocationChange: true }); }
   public showTransferDetails(id: number) { this.router.navigate(['/entries', 'transfer', id], { skipLocationChange: true }); }
   public showEntryNew() { this.router.navigate(['/entries', 'entry', 'new'], { skipLocationChange: true }); }
   public showTransferNew() { this.router.navigate(['/entries', 'transfer', 'new'], { skipLocationChange: true }); }

   // DATA
   public CurrentMonth: Date;
   public CurrentAccount: number = 0;
   public FlowList: EntryFlow[];

   // LOAD FLOW LIST
   public async loadFlowList(): Promise<boolean> {
      try {
         this.busy.show();
         const year = this.CurrentMonth.getFullYear();
         const month = this.CurrentMonth.getMonth() + 1;
         const accountID = this.CurrentAccount;
         let url = `api/entries/flow/${year}/${month}`;
         if (accountID && accountID > 0) { url = `${url}/${accountID}`; }
         this.FlowList = await this.http.get<EntryFlow[]>(url)
            .pipe(map(flows => flows.map(flow => {
               flow.EntryList = flow.EntryList.map(entry => Object.assign(new Entry, entry));
               return Object.assign(new EntryFlow, flow);
            })))
            .toPromise();
         return (this.FlowList != null);
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // ENTRY
   public async getEntry(entryID: number): Promise<Entry> {
      try {
         this.busy.show();
         const dataList = await this.http.get<Entry>(`api/entries/entry/${entryID}`)
            .pipe(map(item => Object.assign(new Entry, item)))
            .toPromise();
         console.log('getEntry', dataList)
         return dataList;
      }
      catch (ex) { console.error(ex); return null; }
      finally { this.busy.hide(); }
   }

}
