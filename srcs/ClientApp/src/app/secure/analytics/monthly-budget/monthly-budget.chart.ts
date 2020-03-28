import { Injectable } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { MonthlyBudgetVM } from '../analytics.viewmodels';
import { TranslationService } from 'src/app/shared/translation/translation.service';

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
export class MonthlyBudgetChart {

   constructor(private translation: TranslationService, private service: AnalyticsService,
      private media: MediaMatcher) {
      this.mobileQuery = this.media.matchMedia('(max-width: 960px)');
   }
   public mobileQuery: MediaQueryList

   public async show(data: MonthlyBudgetVM[]) {
      try {
         const options = await this.options(data);
         Highcharts.chart('monthlyBudgetContainer', options);
      }
      catch (ex) { console.error(ex); }
   }

   private async options(data: MonthlyBudgetVM[]): Promise<Highcharts.Options> {
      return {
         chart: this.chartOptions(),
         title: await this.titleOptions(),
         plotOptions: this.plotOptions(),
         xAxis: this.xAxisOptions(data),
         yAxis: await this.yAxisOptions(data),
         series: this.seriesOptions(data),
         tooltip: this.tooltipOptions(),
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

   private tooltipOptions(): Highcharts.TooltipOptions {
      const self = this;
      return {
         shared: true,
         formatter: function () {
            return `
               <strong>${this.points[0].key}</strong>
               <br/>
               <small>${self.translation.getNumberFormat(this.points[0].y, 2)}</small>
               `;
         }
      };
   }

   private plotOptions(): Highcharts.PlotOptions {
      return {
         column: {
            stacking: 'normal',
            groupPadding: 0.1,
            pointPadding: 0,
            borderWidth: 0
         }
      };
   }

   private xAxisOptions(data: MonthlyBudgetVM[]): Highcharts.XAxisOptions {
      let max = (this.mobileQuery.matches ? 10 : 25);
      if (max > (data && (data.length - 1))) { max = data.length - 1 }
      return {
         type: 'category',
         crosshair: true,
         title: { text: null },
         max: max,
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

   private async yAxisOptions(data: MonthlyBudgetVM[]): Promise<Highcharts.YAxisOptions[]> {
      const min = (data && data[data.length - 1] && data[data.length - 1].OverflowValue) | 0;
      const max = (data && data[0] && data[0].OverflowValue) | 100;
      return [{
         title: {
            text: ''
         },
         min: min,
         max: max,
         gridLineWidth: 1,
         tickPositions: [min, 0, max],
         labels: {
            enabled: false
         }
      }]
   }

   private seriesOptions(data: MonthlyBudgetVM[]): Highcharts.SeriesOptionsType[] {
      const self = this;
      const dataSeries: Highcharts.SeriesOptionsType = {
         name: "Entries",
         type: "column",
         data: data
            .map(x => ({
               name: x.Text,
               color: self.service.Colors.GetCategoryColor(x.CategoryID),
               y: x.OverflowValue
            })
            ),
         zIndex: 2
      };
      return [dataSeries];
   }

}
