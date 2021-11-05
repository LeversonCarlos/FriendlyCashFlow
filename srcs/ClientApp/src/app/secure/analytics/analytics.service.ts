import { Injectable, EventEmitter } from '@angular/core';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { FilterData, CategoryGoalsVM, EntriesParetoVM, MonthlyTargetVM, MonthlyBudgetVM, PatrimonyVM, PatrimonyResumeItem, ApplicationYieldVM } from './analytics.viewmodels';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';
import { Router } from '@angular/router';
import { AnalyticsColors } from './analytics.colors';
import { retry } from 'rxjs/internal/operators/retry';

@Injectable({
   providedIn: 'root'
})
export class AnalyticsService {

   constructor(private busy: BusyService, private appInsights: AppInsightsService,
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
      catch (ex) { this.appInsights.trackException(ex); console.error(ex) }
      finally { this.busy.hide(); }
   }
   public OnDataRefreshed: EventEmitter<boolean> = new EventEmitter<boolean>();

   /* CATEGORY GOALS */
   public CategoryGoals: CategoryGoalsVM[] = null
   public async LoadCategoryGoals(year: number, month: number): Promise<boolean> {
      const url = `api/analytics/categoryGoals/${year}/${month}`;
      this.CategoryGoals = await this.HttpGetWithRetry<CategoryGoalsVM[]>(url);
      return (this.CategoryGoals != null);
   }

   /* ENTRIES PARETO */
   public EntriesPareto: EntriesParetoVM[] = null
   public async LoadEntriesPareto(year: number, month: number): Promise<boolean> {
      const url = `api/analytics/entriesPareto/${year}/${month}`;
      this.EntriesPareto = await this.HttpGetWithRetry<EntriesParetoVM[]>(url);
      return (this.EntriesPareto != null);
   }

   /* MONTHLY TARGET */
   public MonthlyTarget: MonthlyTargetVM[] = null
   public async LoadMonthlyTarget(year: number, month: number): Promise<boolean> {
      const url = `api/analytics/monthlyTarget/${year}/${month}`;
      this.MonthlyTarget = await this.HttpGetWithRetry<MonthlyTargetVM[]>(url);
      return (this.MonthlyTarget != null);
   }

   /* MONTHLY BUDGET */
   public MonthlyBudget: MonthlyBudgetVM[] = null
   /*
   public async LoadMonthlyBudget(year: number, month: number): Promise<boolean> {
      const url = `api/analytics/monthlyBudget/${year}/${month}`;
      this.MonthlyBudget = await this.HttpGetWithRetry<MonthlyBudgetVM[]>(url);
      return (this.MonthlyBudget != null);
   }
   */

   /* APPLICATION YIELD */
   public ApplicationYield: ApplicationYieldVM[] = null
   public async LoadApplicationYield(year: number, month: number): Promise<boolean> {
      const url = `api/analytics/applicationYield/${year}/${month}`;
      this.ApplicationYield = await this.HttpGetWithRetry<ApplicationYieldVM[]>(url);
      return (this.ApplicationYield != null);
   }

   /* PATRIMONY */
   public Patrimony: PatrimonyVM = null
   public async LoadPatrimony(year: number, month: number): Promise<boolean> {
      const url = `api/analytics/patrimony/${year}/${month}`;
      this.Patrimony = await this.HttpGetWithRetry<PatrimonyVM>(url);
      this.Patrimony.PatrimonyResume =
         this.Patrimony.PatrimonyResume.map(x => Object.assign(new PatrimonyResumeItem, x));
      return (this.Patrimony != null);
   }

   private HttpGetWithRetry<T>(url: string): Promise<T> {
      return this.http
         .get<T>(url)
         .pipe(retry(2))
         .toPromise();
   }

}
