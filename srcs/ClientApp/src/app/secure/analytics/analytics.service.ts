import { Injectable, EventEmitter } from '@angular/core';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { HttpClient } from '@angular/common/http';
import { FilterData, CategoryGoalsVM, EntriesParetoVM, MonthlyTargetVM } from './analytics.viewmodels';
import { AppInsightsService } from 'src/app/shared/app-insights/app-insights.service';
import { Router } from '@angular/router';
import { AnalyticsColors } from './analytics.colors';

@Injectable({
   providedIn: 'root'
})
export class AnalyticsService {

   constructor(private busy: BusyService, private appInsights: AppInsightsService,
      private router: Router, private http: HttpClient) { }

   // NAVIGATES
   public showIndex(year: number, month: number) { this.router.navigate(['/analytics', year, month]); }

   /* COLORS */
   public Colors: AnalyticsColors = new AnalyticsColors();

   // FILTER DATA
   public FilterData: FilterData = new FilterData()
   public async setFilter(currentMonth: Date) {
      try {
         this.FilterData.setData(currentMonth);
         const year = (currentMonth && currentMonth.getFullYear()) || 0
         const month = (currentMonth && currentMonth.getMonth() + 1) | 0

         await this.refreshData(year, month)
         this.showIndex(year, month);
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
         this.Colors.Restart();
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
      this.CategoryGoals = await this.http.get<CategoryGoalsVM[]>(url).toPromise();
      return (this.CategoryGoals != null);
   }

   /* ENTRIES PARETO */
   public EntriesPareto: EntriesParetoVM[] = null
   public async LoadEntriesPareto(year: number, month: number): Promise<boolean> {
      const url = `api/analytics/entriesPareto/${year}/${month}`;
      this.EntriesPareto = await this.http.get<EntriesParetoVM[]>(url).toPromise();
      return (this.EntriesPareto != null);
   }

   /* ENTRIES PARETO */
   public MonthlyTarget: MonthlyTargetVM[] = null
   public async LoadMonthlyTarget(year: number, month: number): Promise<boolean> {
      const url = `api/analytics/monthlyTarget/${year}/${month}`;
      this.MonthlyTarget = await this.http.get<MonthlyTargetVM[]>(url).toPromise();
      return (this.MonthlyTarget != null);
   }

}
