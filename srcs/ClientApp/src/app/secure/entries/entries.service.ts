import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { EntryFlow, Entry, CurrentData, enCurrentListType } from './entries.viewmodels';

@Injectable({
   providedIn: 'root'
})
export class EntriesService {

   constructor(private busy: BusyService,
      private http: HttpClient, private router: Router) { }

   // CURRENT DATA
   public CurrentData: CurrentData = new CurrentData()
   public showCurrentList() {
      if (this.CurrentData.ListType == enCurrentListType.Search) {
         const currentSearchText = this.CurrentData.CurrentSearchText || ''
         const currentAccount = this.CurrentData.CurrentAccount || 0
         this.showSearch(currentSearchText, currentAccount)
      }
      else {
         if (this.CurrentData.CurrentMonth) {
            const currentYear = this.CurrentData.CurrentMonth.getFullYear()
            const currentMonth = this.CurrentData.CurrentMonth.getMonth() + 1
            const currentAccount = this.CurrentData.CurrentAccount || 0
            this.showFlow(currentYear, currentMonth, currentAccount);
         }
         else {
            this.showIndex();
         }
      }
   }

   // NAVIGATES
   public showIndex() { this.router.navigate(['/entries']); }
   public showFlow(year: number, month: number, accountID: number = 0) { this.router.navigate(['/entries', 'flow', year, month, accountID]); }
   public showSearch(searchText: string, accountID: number = 0) { this.router.navigate(['/entries', 'search', searchText, accountID]); }
   public showEntryDetails(id: number) { this.router.navigate(['/entries', 'entry', id]); }
   public showTransferDetails(id: string) { this.router.navigate(['/transfer', id]); }
   public showEntryNew(type: string) { this.router.navigate(['/entries', 'entry', 'new', type]); }
   public showTransferNew() { this.router.navigate(['/transfer', 'new']); }

   // FLOW LIST
   public FlowList: EntryFlow[];
   public async loadFlowList(year: number, month: number, accountID: number = 0): Promise<boolean> {
      try {
         this.busy.show();
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
