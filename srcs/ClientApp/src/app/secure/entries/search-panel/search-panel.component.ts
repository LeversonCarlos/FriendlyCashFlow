import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';

@Component({
   selector: 'fs-search-panel',
   templateUrl: './search-panel.component.html',
   styleUrls: ['./search-panel.component.scss']
})
export class SearchPanelComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public ngOnInit() {
   }

   /* CURRENT ACCOUNT */
   public get CurrentAccount(): number {
      return this.service.CurrentData.CurrentAccount || 0;
   }
   public set CurrentAccount(val: number) {
      this.service.setCurrentData(this.CurrentMonth, val)
   }


   /* CURRENT MONTH */
   public get CurrentMonth(): Date {
      return this.service.CurrentData.CurrentMonth;
   }
   public set CurrentMonth(val: Date) {
      this.service.setCurrentData(val, this.CurrentAccount)
   }

}
