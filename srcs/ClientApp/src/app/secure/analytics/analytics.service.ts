import { Injectable, EventEmitter } from '@angular/core';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { FilterData, CategoryGoalsVM, EntriesParetoVM, MonthlyTargetVM, MonthlyBudgetVM, PatrimonyVM, PatrimonyResumeItem, ApplicationYieldVM } from './analytics.viewmodels';
import { Router } from '@angular/router';
import { AnalyticsColors } from './analytics.colors';

@Injectable({
   providedIn: 'root'
})
export class AnalyticsService {

   constructor(private busy: BusyService,
      private router: Router, private http: HttpClient) { }

   // NAVIGATES
   public showIndex(year: number, month: number, isPrinting: boolean = false) {
      if (isPrinting)
         this.router.navigate(['/analytics', year, month, 1]);
      else
         this.router.navigate(['/analytics', year, month]);
   }

   /* COLORS */
   public Colors: AnalyticsColors = new AnalyticsColors();

   // FILTER DATA
   public FilterData: FilterData = new FilterData()
   public async setFilter(currentMonth: Date, isPrinting: boolean = false) {
      try {
         this.FilterData.setData(currentMonth);
         const year = (currentMonth && currentMonth.getFullYear()) || 0
         const month = (currentMonth && currentMonth.getMonth() + 1) || 0

         await this.refreshData(year, month)
         this.showIndex(year, month, isPrinting);
      }
      catch (ex) { console.error(ex); }
   }

   /* REFRESH DATA */
   public async refreshData(year: number, month: number) {
      try {
         if (year == 0 || month == 0) { return false; }
         this.busy.show();
         await this.LoadCategoryGoals(year, month);
         await this.LoadEntriesPareto(year, month);
         await this.LoadMonthlyTarget(year, month);
         await this.LoadApplicationYield(year, month);
         await this.LoadPatrimony(year, month);
         // this.Colors.Restart();
         this.OnDataRefreshed.emit(true);
      }
      catch (ex) { console.error(ex) }
      finally { this.busy.hide(); }
   }
   public OnDataRefreshed: EventEmitter<boolean> = new EventEmitter<boolean>();

   /* CATEGORY GOALS */
   public CategoryGoals: CategoryGoalsVM[] = null
   public async LoadCategoryGoals(year: number, month: number): Promise<boolean> {
      const url = `api/monthlyResult/categoryGoals/${year}/${month}`;
      this.CategoryGoals = await this.http.get<CategoryGoalsVM[]>(url).toPromise();
      return (this.CategoryGoals != null);
   }

   /* ENTRIES PARETO */
   public EntriesPareto: EntriesParetoVM[] = null
   public async LoadEntriesPareto(year: number, month: number): Promise<boolean> {
      const url = `api/monthlyResult/entriesPareto/${year}/${month}`;
      this.EntriesPareto = await this.http.get<EntriesParetoVM[]>(url).toPromise();
      return (this.EntriesPareto != null);
   }

   /* MONTHLY TARGET */
   public MonthlyTarget: MonthlyTargetVM = null
   public async LoadMonthlyTarget(year: number, month: number): Promise<boolean> {
      const url = `api/monthlyResult/monthlyTarget/${year}/${month}`;
      this.MonthlyTarget = await this.http.get<MonthlyTargetVM>(url).toPromise();
      return (this.MonthlyTarget != null);
   }

   /* MONTHLY BUDGET */
   public MonthlyBudget: MonthlyBudgetVM[] = null
   /*
   public async LoadMonthlyBudget(year: number, month: number): Promise<boolean> {
      const url = `api/monthlyResult/monthlyBudget/${year}/${month}`;
      this.MonthlyBudget = await this.http.get<MonthlyBudgetVM[]>(url).toPromise();
      return (this.MonthlyBudget != null);
   }
   */

   /* APPLICATION YIELD */
   public ApplicationYield: ApplicationYieldVM[] = null
   public async LoadApplicationYield(year: number, month: number): Promise<boolean> {
      const url = `api/monthlyResult/applicationYield/${year}/${month}`;
      this.ApplicationYield = await this.http.get<ApplicationYieldVM[]>(url).toPromise();
      return (this.ApplicationYield != null);
   }

   /* PATRIMONY */
   public Patrimony: PatrimonyVM = null
   public async LoadPatrimony(year: number, month: number): Promise<boolean> {
      const url = `api/monthlyResult/patrimony/${year}/${month}`;
      this.Patrimony = await this.http.get<PatrimonyVM>(url).toPromise();
      this.Patrimony.PatrimonyResume =
         this.Patrimony.PatrimonyResume.map(x => Object.assign(new PatrimonyResumeItem, x));
      return (this.Patrimony != null);
   }

}
