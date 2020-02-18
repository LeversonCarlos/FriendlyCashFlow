import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
   selector: 'fs-add-button',
   templateUrl: './add-button.component.html',
   styleUrls: ['./add-button.component.scss']
})
export class AddButtonComponent implements OnInit {

   constructor(private router: Router) { }

   public ngOnInit() {
   }

   public OnIncomeClick() { this.showEntryNew('Income') }
   public OnExpenseClick() { this.showEntryNew('Expense') }
   public OnTransferClick() { this.showTransferNew() }

   private showEntryNew(type: string) { this.router.navigate(['/entries', 'entry', 'new', type], { skipLocationChange: true }); }
   private showTransferNew() { this.router.navigate(['/transfer', 'new'], { skipLocationChange: true }); }

}
