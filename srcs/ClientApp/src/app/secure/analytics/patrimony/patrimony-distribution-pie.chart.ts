import { Injectable } from '@angular/core';
import { PatrimonyDistributionItem } from '../analytics.viewmodels';
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
export class PatrimonyDistributionPieChart {

   constructor(private translation: TranslationService, private service: AnalyticsService) { }

   public async show(data: PatrimonyDistributionItem[]) {
      try {
         const options = await this.options(data);
         Highcharts.chart('patrimonyDistributionContainer', options);
      }
      catch (ex) { console.error(ex); }
   }

   private async options(data: PatrimonyDistributionItem[]): Promise<Highcharts.Options> {
      return {
         chart: this.chartOptions(),
         title: await this.titleOptions(),
         plotOptions: this.plotOptions(),
         series: await this.seriesOptions(data),
         tooltip: this.tooltipOptions(),
         credits: { enabled: false },
         legend: this.legendOptions(),
      };
   }

   private chartOptions(): Highcharts.ChartOptions {
      return {
         type: 'pie',
         backgroundColor: 'transparent'
      };
   }

   private async titleOptions(): Promise<Highcharts.TitleOptions> {
      return {
         text: null
      };
   }

   private tooltipOptions(): Highcharts.TooltipOptions {
      return {
         enabled: false
      };
   }

   private legendOptions(): Highcharts.LegendOptions {
      const self = this;
      return {
         enabled: true,
         labelFormat: '<b>{name}</b>: <small>{y:,.2f}</small>',
         layout: 'vertical',
         verticalAlign: 'top',
         itemStyle: {
            color: self.service.Colors.GetForecolorSchemeSensitive()
         },
         align: 'left'
      };
   }

   private plotOptions(): Highcharts.PlotOptions {
      return {
         pie: {
            dataLabels: {
               enabled: false
            },
            showInLegend: true
         }
      };
   }

   private async seriesOptions(data: PatrimonyDistributionItem[]): Promise<Highcharts.SeriesOptionsType[]> {
      const self = this;
      const dataSeries: Highcharts.SeriesOptionsType = {
         name: await this.translation.getValue("ACCOUNTS_MAIN_TITLE"),
         type: "pie",
         colorByPoint: true,
         data: data
            .map(x => ({
               name: x.Text,
               y: x.Value
            })
            )
      };
      return [dataSeries];
   }

}
