import { Injectable } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { EntriesParetoVM } from '../analytics.viewmodels';
import { TranslationService } from 'src/app/shared/translation/translation.service';

import * as Highcharts from 'highcharts';
import { AnalyticsService } from '../analytics.service';
declare var require: any;
let Boost = require('highcharts/modules/boost');
let noData = require('highcharts/modules/no-data-to-display');
let pareto = require('highcharts/modules/pareto');
let More = require('highcharts/highcharts-more');
Boost(Highcharts);
noData(Highcharts);
More(Highcharts);
noData(Highcharts);
pareto(Highcharts);

@Injectable({
   providedIn: 'root'
})
export class EntriesParetoChart {

   constructor(private translation: TranslationService, private service: AnalyticsService,
      private media: MediaMatcher) {
      this.mobileQuery = this.media.matchMedia('(max-width: 960px)');
   }
   public mobileQuery: MediaQueryList

   public async show(data: EntriesParetoVM[]) {
      try {
         const options = await this.options(data);
         Highcharts.setOptions({
            lang: {
               drillUpText: '<<'
            }
         });
         Highcharts.chart('entriesParetoContainer', options);
      }
      catch (ex) { console.error(ex); }
   }

   private async options(data: EntriesParetoVM[]): Promise<Highcharts.Options> {
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
               <span style="color:${this.points[1].color}">\u25CF</span>
               <span>${this.points[1].key}</span>
               <br/>
               <span style="color:transparent">\u25CF</span>
               <strong>${self.translation.getNumberFormat(this.points[1].y, 2)}</strong>
               <small> (${self.translation.getNumberFormat(100-this.points[0].y, 0)}%)</small>
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

   private xAxisOptions(data: EntriesParetoVM[]): Highcharts.XAxisOptions {
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

   private async yAxisOptions(data: EntriesParetoVM[]): Promise<Highcharts.YAxisOptions[]> {
      return [{
         title: {
            text: ''
         },
         min: 0, max: (data && data[0] && data[0].Value) | 100,
         gridLineWidth: 0,
         labels: {
            enabled: false
         }
      }, {
         title: {
            text: ''
         },
         min: 0, max: 99,
         gridLineWidth: 1,
         labels: {
            enabled: false
         }
      }]
   }

   private seriesOptions(data: EntriesParetoVM[]): Highcharts.SeriesOptionsType[] {
      const self = this;
      const paretoSeries: Highcharts.SeriesOptionsType = {
         name: "Pareto",
         type: "spline",
         yAxis: 1,
         color: self.service.Colors.GetForecolorSchemeSensitive(),
         lineWidth: 1,
         marker: { enabled: true, radius: 2 },
         data: data
            .map(x => ({
               name: x.Text,
               y: x.Pareto * 100
            })),
         zIndex: 10
      };
      const dataSeries: Highcharts.SeriesOptionsType = {
         name: "Entries",
         type: "column",
         data: data
            .map(x => ({
               name: x.Text,
               color: self.service.Colors.GetCategoryColor(x.CategoryID),
               y: x.Value
            })
            ),
         zIndex: 2
      };
      return [paretoSeries, dataSeries];
   }

}
