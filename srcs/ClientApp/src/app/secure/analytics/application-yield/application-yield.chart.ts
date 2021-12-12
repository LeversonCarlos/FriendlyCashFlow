import { Injectable } from '@angular/core';
import { TranslationService } from 'src/app/shared/translation/translation.service';
import { ApplicationYieldVM } from '../analytics.viewmodels';

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
export class ApplicationYieldChart {

   constructor(private translation: TranslationService, private service: AnalyticsService) {
      this.BalanceColor = this.service.Colors.GetForecolorSchemeSensitive();
   }

   private BalanceColor = null;
   private IncomeColor = '#4CAF50';
   private ExpenseColor = '#f44336';
   private GoalColor = 'green';

   public async show(data: ApplicationYieldVM[]) {
      try {
         const options = await this.options(data);
         Highcharts.chart('applicationYieldContainer', options);
      }
      catch (ex) { console.error(ex); }
   }

   private async options(data: ApplicationYieldVM[]): Promise<Highcharts.Options> {
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
            stacking: 'percent'
         }
      };
   }

   private xAxisOptions(data: ApplicationYieldVM[]): Highcharts.XAxisOptions {
      const categoryList = data
         .sort((a, b) => a.Date > b.Date ? 1 : -1)
         .map(x => x.DateText);
      return {
         title: { text: null },
         categories: categoryList,
         tickmarkPlacement: 'on',
         labels: {
            enabled: true,
            reserveSpace: true
         },
         tickLength: 1
      };
   }

   private async yAxisOptions(data: ApplicationYieldVM[]): Promise<Highcharts.YAxisOptions> {
      return {
         title: { text: null },
         gridLineColor: 'transparent',
         tickPositions: [0, 101],
         // max: 101,
         labels: { enabled: false }
      };
   }

   private async tooltipOptions(): Promise<Highcharts.TooltipOptions> {
      const self = this;
      return {
         shared: true,
         formatter: function () {
            const tootipList = this.points
               .map(p => `
                  <span style="color:${p.color}">\u25CF</span>
                  <span>${p.series.name}:</span>
                  <strong>${self.translation.getNumberFormat((p.point.options as any).OriginalGain, 2)}</strong>
                  `);
            const tootip = tootipList.join('<br/>');
            const tootipHeader = `<strong>${this.points[0].key}</strong>`;
            return `${tootipHeader}<br/>${tootip}`;
         }
      };
   }

   private async seriesOptions(data: ApplicationYieldVM[]): Promise<Highcharts.SeriesOptionsType[]> {

      let seriesHash = {};
      let seriesList: Highcharts.SeriesOptionsType[] = [];

      for (let iDate = 0; iDate < data.length; iDate++) {
         const date = data[iDate];
         const accounts = date.Accounts;

         for (let iAccount = 0; iAccount < accounts.length; iAccount++) {
            const account = accounts[iAccount];

            let seriesItem: any = null; //Highcharts.SeriesAreaOptions

            if (!seriesHash.hasOwnProperty(account.AccountText)) {
               seriesHash[account.AccountText] = seriesList.length;

               seriesItem = {
                  name: account.AccountText,
                  type: 'column',
                  yAxis: 0,
                  color: this.service.Colors.GetAccountColor(account.AccountText),
                  data: data
                     .map(date => {
                        return {
                           name: date.DateText,
                           y: 0.0,
                           OriginalGain: 0.0,
                           Gain: 0.0
                        };
                     })
               };
               seriesList.push(seriesItem);

            }

            seriesItem = seriesList[seriesHash[account.AccountText]];
            let dataItem = seriesItem.data[iDate];
            dataItem.y = account.Percentual;
            dataItem.OriginalGain = account.OriginalGain;
            dataItem.Gain = account.Gain;

         }

      }

      return seriesList;
   }

}
