import { Injectable } from '@angular/core';
import { TranslationService } from 'src/app/shared/translation/translation.service';
import { YearlyBudgetVM } from '../analytics.viewmodels';

import * as Highcharts from 'highcharts';
import { AnalyticsService } from '../analytics.service';
declare var require: any;
let Boost = require('highcharts/modules/boost');
let noData = require('highcharts/modules/no-data-to-display');
let More = require('highcharts/highcharts-more');
let drilldown = require('highcharts/modules/drilldown');
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
         console.log(this.service.YearlyBudget);
         const options = await this.options(data);
         Highcharts.chart('yearlyBudgetContainer', options);
      }
      catch (ex) { console.error(ex); }
   }

   private async options(data: YearlyBudgetVM[]): Promise<Highcharts.Options> {
      return {
         chart: this.chartOptions(),
         title: await this.titleOptions(),
         // plotOptions: this.plotOptions(),
         // xAxis: this.xAxisOptions(data),
         // yAxis: await this.yAxisOptions(data),
         series: this.seriesOptions(data),
         // tooltip: await this.tooltipOptions(),
         credits: { enabled: false },
         legend: { enabled: false },
      };
   }

   private chartOptions(): Highcharts.ChartOptions {
      return {
         type: 'column',
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
            groupPadding: 0.1,
            pointPadding: 0,
            //pointWidth: 20,
            borderWidth: 0
         }
      };
   }

   private xAxisOptions(data: YearlyBudgetVM[]): Highcharts.XAxisOptions {
      return {
         type: 'category',
         /* categories: categoryList, */
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

   private async yAxisOptions(data: YearlyBudgetVM[]): Promise<Highcharts.YAxisOptions> {
      let maxValue = data
         .map(x => x.MonthPercentage)
         .sort((a, b) => a < b ? 1 : -1)
         .reduce((a, b) => a || b, 0) || 0;
      maxValue = maxValue < 105 ? 105 : maxValue;
      return {
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
               style: { fontSize: '1.3vh', color: this.service.Colors.GetForecolorSchemeSensitive() }
            },
            zIndex: 7,
            width: 2
         }],
         labels: { enabled: false }
      };
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

   private seriesOptions(data: YearlyBudgetVM[]): Highcharts.SeriesOptionsType[] {
      const self = this;
      const seriesList: Highcharts.SeriesOptionsType = {
         name: 'Categories',
         type: 'column',
         data: data
            //.sort((a, b) => a.Text > b.Text ? 1 : -1)
            .map(x => ({
               name: x.CategoryText,
               color: self.service.Colors.GetCategoryColor(x.CategoryID),
               y: x.MonthPercentage,
               goalValue: x.BudgetValue,
               realValue: x.MonthValue
            }))
      };
      return [seriesList];
   }

}
