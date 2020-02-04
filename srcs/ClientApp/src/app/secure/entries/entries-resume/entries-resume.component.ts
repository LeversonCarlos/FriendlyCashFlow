import { Component, OnInit } from '@angular/core';
import { EntriesService } from '../entries.service';

class EntriesResumeVM {
   Income: number = 0
   Expense: number = 0
}

@Component({
   selector: 'fs-entries-resume',
   templateUrl: './entries-resume.component.html',
   styleUrls: ['./entries-resume.component.scss']
})
export class EntriesResumeComponent implements OnInit {

   constructor(private service: EntriesService) { }

   public get EntriesResume(): EntriesResumeVM {
      return this.service.FlowList &&
         this.service.FlowList
            .map(x => x.EntryList)
            .reduce((a, b) => a.concat(b), [])
            .filter(x => x.EntryID > 0 && (x.TransferID == null || x.TransferID == ''))
            .map(x => Object.assign({}, {
               Income: (x.EntryValue > 0 ? x.EntryValue : 0),
               Expense: (x.EntryValue < 0 ? x.EntryValue : 0)
            }))
            .reduce((a, b) => Object.assign(new EntriesResumeVM, {
               Income: Math.round((a.Income + b.Income) * 100) / 100,
               Expense: Math.round((a.Expense + b.Expense) * 100) / 100
            }), new EntriesResumeVM);
   }

   public get IncomeValue(): number { return this.EntriesResume.Income };
   public get ExpenseValue(): number { return this.EntriesResume.Expense };
   public get BalanceValue(): number { return this.EntriesResume.Income + this.EntriesResume.Expense };


   ngOnInit() {
   }

}
