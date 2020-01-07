import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';

@Component({
   selector: 'fs-add-button',
   templateUrl: './add-button.component.html',
   styleUrls: ['./add-button.component.scss']
})
export class AddButtonComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public ngOnInit() {
   }

   public OnIncomeClick() { this.service.showEntryNew('Income') }
   public OnExpenseClick() { this.service.showEntryNew('Expense') }
   public OnTransferClick() { this.service.showTransferNew() }

}
