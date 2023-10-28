import { Injectable } from '@angular/core';
import { TranslationService } from 'src/app/shared/translation/translation.service';
import { YearlyBudgetVM } from '../analytics.viewmodels';

import * as Highcharts from 'highcharts';
import { AnalyticsService } from '../analytics.service';
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
export class YearlyBudgetChart {

   constructor(private service: AnalyticsService, private translation: TranslationService) { }

   public async show(data: YearlyBudgetVM[]) {
      try {
         const options = await this.options(data);
         Highcharts.chart('yearlyBudgetContainer', options);
      }
      catch (ex) { console.error(ex); }
   }

   private async options(data: YearlyBudgetVM[]): Promise<Highcharts.Options> {
      return {
         chart: this.chartOptions(),
         title: await this.titleOptions(),
         plotOptions: this.plotOptions(),
         xAxis: this.xAxisOptions(data),
         yAxis: await this.yAxisOptions(data),
         series: this.seriesOptions(data),
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
            //stacking: 'normal',
            groupPadding: 0.1,
            pointPadding: 0,
            //pointWidth: 20,
            borderWidth: 1
         }
      };
   }

   private xAxisOptions(data: YearlyBudgetVM[]): Highcharts.XAxisOptions {
      return {
         type: 'category',
         categories: data
            .map(x => x.CategoryText),
         title: { text: null },
         labels: {
            rotation: -90,
            enabled: true,
            reserveSpace: false,
            align: 'left',
            y: -5,
            style: {
               textOverflow: 'none',
               whiteSpace: 'nowrap',
               color: "#000",
               textShadow: '0px 0px 5px white'
            }
         },
         tickWidth: 0
      };
   }

   private async yAxisOptions(data: YearlyBudgetVM[]): Promise<Highcharts.YAxisOptions[]> {

      /*
      let monthlyMax = data
         .map(x => x.MonthPercentage)
         .sort((a, b) => a < b ? 1 : -1)
         .reduce((a, b) => a || b, 0) || 0;
      monthlyMax = monthlyMax < 105 ? 105 : monthlyMax;
      monthlyMax = monthlyMax > 200 ? 200 : monthlyMax;

      let yearlyMax = data
         .map(x => x.MonthPercentage)
         .sort((a, b) => a < b ? 1 : -1)
         .reduce((a, b) => a || b, 0) || 0;
      yearlyMax = yearlyMax < 105 ? 105 : yearlyMax;
      yearlyMax = yearlyMax > 200 ? 200 : yearlyMax;
      */

      const getOptions = async (max: number) => {
         return {
            title: { text: null },
            gridLineColor: 'transparent',
            tickPositions: [0, 100, max],
            max: max,
            plotLines: [{
               value: 100,
               color: 'green',
               label: {
                  text: await this.translation.getValue("ANALYTICS_CATEGORY_GOALS_GOAL_LABEL"),
                  x: 0,
                  style: { fontSize: '1.3vh', color: this.service.Colors.GetForecolorSchemeSensitive() }
               },
               zIndex: 7,
               width: 2
            }],
            labels: { enabled: false }
         }
      };

      return [
         await getOptions(130),
         {
            title: { text: null },
            gridLineColor: 'transparent',
            max: 130,
            labels: { enabled: false }
         },
         {
            title: { text: null },
            gridLineColor: 'transparent',
            labels: { enabled: false }
         }
      ];
   }

   private async tooltipOptions(): Promise<Highcharts.TooltipOptions> {
      const self = this;
      const goalLabel = await this.translation.getValue("ANALYTICS_YEARLY_BUDGET_GOAL_LABEL");
      const monthLabel = await this.translation.getValue("ANALYTICS_YEARLY_BUDGET_MONTH_LABEL");
      const yearLabel = await this.translation.getValue("ANALYTICS_YEARLY_BUDGET_YEAR_LABEL");
      const unexpectedLabel = await this.translation.getValue("ANALYTICS_YEARLY_BUDGET_UNEXPECTED_LABEL");
      return {
         shared: true,
         formatter: function () {
            let tooltipResult = '';
            let tootipPointName = '';
            this.points.forEach(p => {
               const point: any = p.point;
               tootipPointName = point.name;
               if (point.BudgetValue > 0) {
                  tooltipResult =
                     '<br/>' +
                     '<span style="color:' + 'green' + '">\u25CF</span> ' +
                     '<span>' + goalLabel + '</span>: ' +
                     '<strong>' + self.translation.getNumberFormat(point.BudgetValue, 2) + '</strong>' +
                     tooltipResult;
               }
               if (point.MonthValue > 0) {
                  tooltipResult +=
                     '<br/>' +
                     '<span>\u25CF</span> ' +
                     '<span>' + monthLabel + '</span>: ' +
                     '<strong>' + self.translation.getNumberFormat(point.MonthValue, 2) + '</strong>';
               }
               if (point.YearValue > 0) {
                  tooltipResult +=
                     '<br/>' +
                     '<span>\u25CF</span> ' +
                     '<span>' + yearLabel + '</span>: ' +
                     '<strong>' + self.translation.getNumberFormat(point.YearValue, 2) + '</strong>';
               }
               if (point.UnexpectedValue > 0) {
                  tooltipResult +=
                     '<br/>' +
                     '<span style="color:' + 'red' + '">\u25CF</span> ' +
                     '<span>' + unexpectedLabel + '</span>: ' +
                     '<strong>' + self.translation.getNumberFormat(point.UnexpectedValue, 2) + '</strong>';
               }
            });
            tooltipResult = '<strong>' + tootipPointName + '</strong>' + tooltipResult;
            return tooltipResult;
         }
      };
   }

   private seriesOptions(data: YearlyBudgetVM[]): Highcharts.SeriesOptionsType[] {
      const self = this;

      const monthSeries: Highcharts.SeriesOptionsType = {
         name: 'Month',
         type: 'column',
         yAxis: 0,
         data: data
            .filter(x => x.BudgetValue > 0 && x.MonthValue > 0)
            // .sort((a, b) => a.Text > b.Text ? 1 : -1)
            .map(x => ({
               name: x.CategoryText,
               color: self.service.Colors.GetCategoryColor(x.CategoryID),
               y: x.MonthPercentage,
               BudgetValue: (x.BudgetValue ?? 0),
               MonthValue: x.MonthValue
            }))
      };

      const yearSeries: Highcharts.SeriesOptionsType = {
         name: 'Year',
         type: 'column',
         yAxis: 1,
         data: data
            .filter(x => x.BudgetValue > 0 && x.YearValue > 0)
            // .sort((a, b) => a.Text > b.Text ? 1 : -1)
            .map(x => ({
               name: x.CategoryText,
               color: self.service.Colors.GetCategoryColor(x.CategoryID),
               y: x.YearPercentage,
               YearValue: x.YearValue
            }))
      };

      const unexpectedSeries: Highcharts.SeriesOptionsType = {
         name: 'Unexpected',
         type: 'column',
         yAxis: 2,
         data: data
            .filter(x => (x.BudgetValue == undefined || x.BudgetValue == null || x.BudgetValue == 0) && x.MonthValue > 0)
            // .sort((a, b) => a.Text > b.Text ? 1 : -1)
            .map(x => ({
               name: x.CategoryText,
               color: '#f00',
               y: x.MonthValue,
               UnexpectedValue: x.MonthValue
            }))
      };

      return [monthSeries, yearSeries, unexpectedSeries];
   }

}
