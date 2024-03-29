import { Injectable } from '@angular/core';
import { TranslationService } from 'src/app/shared/translation/translation.service';
import { CategoryGoalsVM } from '../analytics.viewmodels';

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
drilldown(Highcharts);

@Injectable({
   providedIn: 'root'
})
export class CategoryGoalsChart {

   constructor(private service: AnalyticsService, private translation: TranslationService) { }

   public async show(data: CategoryGoalsVM[]) {
      try {
         const options = await this.options(data);
         Highcharts.setOptions({
            lang: {
               drillUpText: '<<'
            }
         });
         Highcharts.chart('categoryGoalsContainer', options);
      }
      catch (ex) { console.error(ex); }
   }

   private async options(data: CategoryGoalsVM[]): Promise<Highcharts.Options> {
      return {
         chart: this.chartOptions(),
         title: await this.titleOptions(),
         plotOptions: this.plotOptions(),
         xAxis: this.xAxisOptions(data),
         yAxis: await this.yAxisOptions(data),
         series: this.seriesOptions(data),
         drilldown: this.drilldownOptions(data),
         tooltip: await this.tooltipOptions(),
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

   private xAxisOptions(data: CategoryGoalsVM[]): Highcharts.XAxisOptions {
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

   private async yAxisOptions(data: CategoryGoalsVM[]): Promise<Highcharts.YAxisOptions> {
      let maxValue = data
         .map(x => x.CategoryPercent)
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

   private seriesOptions(data: CategoryGoalsVM[]): Highcharts.SeriesOptionsType[] {
      const self = this;
      const seriesList: Highcharts.SeriesOptionsType = {
         name: 'Categories',
         type: 'column',
         data: data
            .sort((a, b) => a.Text > b.Text ? 1 : -1)
            .map(x => ({
               name: x.Text,
               color: self.service.Colors.GetCategoryColor(x.CategoryID),
               y: x.CategoryPercent,
               goalValue: x.AverageValue,
               realValue: x.CategoryValue,
               drilldown: x.CategoryID.toString()
            }))
      };
      return [seriesList];
   }

   private drilldownOptions(data: CategoryGoalsVM[]): Highcharts.DrilldownOptions {
      const drilldown = data
         .map(x => ({
            name: x.Text,
            type: null,
            colorByPoint: false,
            id: x.CategoryID.toString(),
            data: x.Childs
               .filter(child => child.CategoryPercent > 0)
               .map(child => ({
                  name: child.Text,
                  y: child.CategoryPercent,
                  goalValue: child.AverageValue,
                  realValue: child.CategoryValue
               }))
         }))
      return {
         drillUpButton: {
            relativeTo: 'spacingBox',
            position: {
               y: 0,
               x: 0
            }
         },
         series: drilldown
      };
   }

}
