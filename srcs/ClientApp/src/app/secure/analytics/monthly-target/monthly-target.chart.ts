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
         series: this.seriesOptions(data),
         tooltip: await this.tooltipOptions(),
         colors: this.colorOptions(),
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
            stacking: 'normal',
            /* colorByPoint: true, */
            groupPadding: 0.1,
            pointPadding: 0,
            //pointWidth: 20,
            borderWidth: 0
         }
      };
   }

   private colorOptions(): string[] {
      return [
         '#2196F3',
         '#4CAF50',
         '#f44336',
         '#FFC107',
         '#3F51B5',
         '#795548',
         '#009688',
         '#FF5722',
         '#FFEB3B',
         '#3F51B5',
         '#607D8B',
         '#8BC34A',
         '#9C27B0',
         '#00BCD4',
         '#FF9800'
      ];
   }

   private xAxisOptions(data: MonthlyTargetVM[]): Highcharts.XAxisOptions {
      /*
      const categoryList = data
         .map(x => x.Text)
         .sort((a, b) => a > b ? 1 : -1);
      */
      return {
         title: { text: null },
         labels: {
            rotation: -90,
            enabled: true,
            reserveSpace: false,
            align: 'left',
            y: -5,
            style: {
               color: "#000",
               textShadow: '0px 0px 5px white'
            }
         },
         tickWidth: 0
      };
   }

   private async yAxisOptions(data: MonthlyTargetVM[]): Promise<Highcharts.YAxisOptions[]> {
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
            color: 'green',
            label: {
               text: await this.translation.getValue("ANALYTICS_CATEGORY_GOALS_GOAL_LABEL"),
               x: 0,
               style: { fontSize: '1.3vh' }
            },
            zIndex: 7,
            width: 2
         }],
         labels: { enabled: false }
      }, {
         title: { text: null },
         labels: { enabled: false }
      }];
   }

   private async tooltipOptions(): Promise<Highcharts.TooltipOptions> {
      const self = this;
      const goalLabel = await this.translation.getValue("ANALYTICS_CATEGORY_GOALS_GOAL_LABEL");
      const valueLabel = await this.translation.getValue("ANALYTICS_CATEGORY_GOALS_VALUE_LABEL");
      return {
         shared: true,
         formatter: function () {
            let tooltipResult = '';
            let tootipPointName = '';
            this.points.forEach(p => {
               const point: any = p.point;
               tootipPointName = point.name;
               if (point.goalValue > 0) {
                  tooltipResult =
                     '<br/>' +
                     '<span style="color:' + 'green' + '">\u25CF</span> ' +
                     '<span>' + goalLabel + '</span>: ' +
                     '<strong>' + self.translation.getNumberFormat(point.goalValue, 2) + '</strong>' +
                     tooltipResult;
               }
               tooltipResult +=
                  '<br/>' +
                  '<span>\u25CF</span> ' +
                  '<span>' + valueLabel + '</span>: ' +
                  '<strong>' + self.translation.getNumberFormat(point.realValue, 2) + '</strong>';
            });
            tooltipResult = '<strong>' + tootipPointName + '</strong>' + tooltipResult;
            return tooltipResult;
         }
      };
   }

   private seriesOptions(data: MonthlyTargetVM[]): Highcharts.SeriesOptionsType[] {
      const incomeSeries: Highcharts.SeriesOptionsType = {
         name: 'Income',
         type: 'column',
         yAxis: 0,
         data: data
            .sort((a, b) => a.SearchDate > b.SearchDate ? 1 : -1)
            .map(x => ({
               name: x.Text,
               y: Math.round((x.IncomeValue / x.IncomeAverage * 100) * 100) / 100,
               goalValue: x.IncomeTarget,
               realValue: x.IncomeValue
            }))
      };
      const expenseSeries: Highcharts.SeriesOptionsType = {
         name: 'Expense',
         type: 'column',
         yAxis: 0,
         data: data
            .sort((a, b) => a.SearchDate > b.SearchDate ? 1 : -1)
            .map(x => ({
               name: x.Text,
               y: Math.round((x.ExpenseValue / x.ExpenseAverage * 100) * 100) / 100,
               goalValue: x.ExpenseTarget,
               realValue: x.ExpenseValue
            }))
      };
      return [incomeSeries, expenseSeries];
   }

}
