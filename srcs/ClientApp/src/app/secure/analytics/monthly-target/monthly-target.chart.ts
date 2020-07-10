import { Injectable } from '@angular/core';
import { TranslationService } from 'src/app/shared/translation/translation.service';
import { MonthlyTargetVM } from '../analytics.viewmodels';

import * as Highcharts from 'highcharts';
declare var require: any;
let Boost = require('highcharts/modules/boost');
let noData = require('highcharts/modules/no-data-to-display');
let More = require('highcharts/highcharts-more');
Boost(Highcharts);
noData(Highcharts);
More(Highcharts);
noData(Highcharts);

@Injectable({
   providedIn: 'root'
})
export class MonthlyTargetChart {

   constructor(private translation: TranslationService) { }

   private BalanceColor = '#333';
   private IncomeColor = '#4CAF50';
   private ExpenseColor = '#f44336';
   private GoalColor = 'green';

   public async show(data: MonthlyTargetVM[]) {
      try {
         const options = await this.options(data);
         Highcharts.chart('monthlyTargetContainer', options);
      }
      catch (ex) { console.error(ex); }
   }

   private async options(data: MonthlyTargetVM[]): Promise<Highcharts.Options> {
      return {
         chart: this.chartOptions(),
         title: await this.titleOptions(),
         plotOptions: this.plotOptions(),
         xAxis: this.xAxisOptions(data),
         yAxis: await this.yAxisOptions(data),
         series: await this.seriesOptions(data),
         tooltip: await this.tooltipOptions(),
         credits: { enabled: false },
         legend: { enabled: false },
      };
   }

   private chartOptions(): Highcharts.ChartOptions {
      return {
         backgroundColor: 'transparent'
      };
   }

   private async titleOptions(): Promise<Highcharts.TitleOptions> {
      return {
         text: null
      };
   }

   private plotOptions(): Highcharts.PlotOptions {
      return {
         column: {
            groupPadding: 0.1,
            pointPadding: 0,
            //pointWidth: 20,
            borderWidth: 0
         }
      };
   }

   private xAxisOptions(data: MonthlyTargetVM[]): Highcharts.XAxisOptions {
      const categoryList = data
         .sort((a, b) => a.SearchDate > b.SearchDate ? 1 : -1)
         .map(x => x.SmallText);
      return {
         title: { text: null },
         categories: categoryList,
         tickmarkPlacement: 'on',
         labels: { enabled: true },
         tickLength: 1
      };
   }

   private async yAxisOptions(data: MonthlyTargetVM[]): Promise<Highcharts.YAxisOptions[]> {
      const maxBalance = data
         .map(x => x.Balance)
         .sort((a, b) => a < b ? 1 : -1)
         .reduce((a, b) => a || b, 0) || 0;
      let maxValue = data
         .map(x => x.IncomeTarget > x.ExpenseTarget ? x.IncomeTarget : x.ExpenseTarget)
         .sort((a, b) => a < b ? 1 : -1)
         .reduce((a, b) => a || b, 0) || 0;
      maxValue = maxValue < 105 ? 105 : maxValue;
      return [{
         title: { text: null },
         gridLineColor: 'transparent',
         tickPositions: [0, 100, maxValue],
         max: maxValue,
         plotLines: [{
            value: 100,
            color: this.GoalColor,
            label: {
               text: await this.translation.getValue("ANALYTICS_MONTHLY_TARGET_GOAL_LABEL"),
               x: 0,
               style: { fontSize: '1.3vh' }
            },
            zIndex: 7,
            width: 2
         }],
         labels: { enabled: false }
      }, {
         title: { text: null },
         gridLineColor: 'transparent',
         tickPositions: [0, maxBalance],
         max: maxBalance,
         labels: { enabled: false }
      }];
   }

   private async tooltipOptions(): Promise<Highcharts.TooltipOptions> {
      const self = this;
      const goalLabel = await this.translation.getValue("ANALYTICS_MONTHLY_TARGET_GOAL_LABEL");
      return {
         shared: true,
         formatter: function () {
            const incomePoint: any = this.points[0].point;
            const incomeText = `<br/>
               <span style="color:${self.IncomeColor}">\u25CF</span>
               <span>${incomePoint.series.name}</span>
               <strong>${self.translation.getNumberFormat(incomePoint.realValue, 2)}</strong>
               `
            const expensePoint: any = this.points[1].point;
            const expenseText = `<br/>
               <span style="color:${self.ExpenseColor}">\u25CF</span>
               <span>${expensePoint.series.name}</span>
               <strong>${self.translation.getNumberFormat(expensePoint.realValue, 2)}</strong>
               `
            const goalText = `<br/>
               <span style="color:transparent">\u25CF</span>
               <small>(</small>
               <small>${goalLabel}</small>
               <small>${self.translation.getNumberFormat(expensePoint.goalValue, 2)}</small>
               <small>)</small>
            `
            const balancePoint: any = this.points[2].point;
            const balanceText = `<br/>
                  <span style="color:${self.BalanceColor}">\u25CF</span>
                  <span>${balancePoint.series.name}</span>
                  <strong>${self.translation.getNumberFormat(balancePoint.y, 2)}</strong>
                  `
            const tooltip = `<strong>${incomePoint.name}</strong>${incomeText}${expenseText}${goalText}${balanceText}`;
            return tooltip;
         }
      };
   }

   private async seriesOptions(data: MonthlyTargetVM[]): Promise<Highcharts.SeriesOptionsType[]> {
      const incomeSeries: Highcharts.SeriesOptionsType = {
         name: await this.translation.getValue('ANALYTICS_MONTHLY_TARGET_INCOME_LABEL'),
         type: 'column',
         yAxis: 0,
         color: this.IncomeColor,
         data: data
            .sort((a, b) => a.SearchDate > b.SearchDate ? 1 : -1)
            .map(x => ({
               name: x.FullText,
               y: x.IncomeTarget,
               goalValue: x.IncomeAverage,
               realValue: x.IncomeValue
            }))
      };
      const expenseSeries: Highcharts.SeriesOptionsType = {
         name: await this.translation.getValue('ANALYTICS_MONTHLY_TARGET_EXPENSE_LABEL'),
         type: 'column',
         yAxis: 0,
         color: this.ExpenseColor,
         data: data
            .sort((a, b) => a.SearchDate > b.SearchDate ? 1 : -1)
            .map(x => ({
               name: x.FullText,
               y: x.ExpenseTarget,
               goalValue: x.ExpenseAverage,
               realValue: x.ExpenseValue
            }))
      };
      const balanceSeries: Highcharts.SeriesOptionsType = {
         name: await this.translation.getValue('ANALYTICS_MONTHLY_TARGET_BALANCE_LABEL'),
         type: 'line',
         yAxis: 1,
         color: this.BalanceColor,
         lineWidth: 1,
         marker: { enabled: true, radius: 2 },
         data: data
            .map(x => ({
               name: x.FullText,
               y: x.Balance
            })),
         zIndex: 10
      };
      return [incomeSeries, expenseSeries, balanceSeries];
   }

}
