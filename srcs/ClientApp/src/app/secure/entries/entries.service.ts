import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { EntryFlow, Entry } from './entries.viewmodels';

@Injectable({
   providedIn: 'root'
})
export class EntriesService {

   constructor(private busy: BusyService,
      private http: HttpClient, private router: Router) { }

   // NAVIGATES
   public showFlow(year?: number, month?: number) {
      if (year == undefined && this.CurrentMonth) { year = this.CurrentMonth.getFullYear(); }
      if (month == undefined && this.CurrentMonth) { month = this.CurrentMonth.getMonth() + 1; }
      if (year == undefined || month == undefined) { this.router.navigate(['/entries']); }
      this.router.navigate(['/entries', 'flow', year, month]);
   }
   public showSearch(searchText: string, accountID: number = 0) { this.router.navigate(['/entries', 'search', searchText, accountID]); }
   public showEntryDetails(id: number) { this.router.navigate(['/entries', 'entry', id]); }
   public showTransferDetails(id: number) { this.router.navigate(['/entries', 'transfer', id]); }
   public showEntryNew(type: string) { this.router.navigate(['/entries', 'entry', 'new', type]); }
   public showTransferNew() { this.router.navigate(['/entries', 'transfer', 'new']); }

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
         const data = await this.http.get<Entry>(`api/entries/entry/${entryID}`)
            .pipe(map(item => Object.assign(new Entry, item)))
            .toPromise();
         return data;
      }
      catch (ex) { console.error(ex); return null; }
      finally { this.busy.hide(); }
   }

   // SAVE
   public async saveEntry(value: Entry): Promise<boolean> {
      try {
         this.busy.show();
         let result: Entry = null;
         if (!value.EntryID || value.EntryID == 0) {
            result = await this.http.post<Entry>(`api/entries`, value).toPromise();
         }
         else {
            result = await this.http.put<Entry>(`api/entries/${value.EntryID}`, value).toPromise();
         }
         return result != null;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // REMOVE
   public async removeEntry(value: Entry): Promise<boolean> {
      try {
         this.busy.show();
         const result = await this.http.delete<boolean>(`api/entries/${value.EntryID}`).toPromise();
         return result;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

}
