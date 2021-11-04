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
         type: 'area',
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
         area: {
            stacking: 'percent',
            lineColor: '#666666',
            lineWidth: 1,
            marker: {
               lineWidth: 1,
               lineColor: '#666666'
            }
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
         labels: { enabled: true },
         tickLength: 1
      };
   }

   private async yAxisOptions(data: ApplicationYieldVM[]): Promise<Highcharts.YAxisOptions> {
      return {
         title: { text: null },
         gridLineColor: 'transparent',
         // tickPositions: [0, 100, maxValue],
         // max: maxValue,
         labels: { enabled: false }
      };
   }

   private async tooltipOptions(): Promise<Highcharts.TooltipOptions> {
      const self = this;
      return {
         shared: true/*,
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
            const balancePoint: any = this.points[2].point;
            const balanceText = `<br/>
                  <span style="color:${self.BalanceColor}">\u25CF</span>
                  <span>${balancePoint.series.name}</span>
                  <strong>${self.translation.getNumberFormat(balancePoint.y, 2)}</strong>
                  `
            const tooltip = `<strong>${incomePoint.name}</strong>${incomeText}${expenseText}${balanceText}`;
            return tooltip;
         }
         */
      };
   }

   private async seriesOptions(data: ApplicationYieldVM[]): Promise<Highcharts.SeriesOptionsType[]> {
      console.log('data', data);

      let seriesHash = {};
      let seriesList: Highcharts.SeriesOptionsType[] = [];

      let emptyDataList: Highcharts.PointOptionsObject[] = data
         .map(date => {
            return {
               name: date.DateText,
               y: 0.0,
               GainValue: 0.0
            };
         });
      console.log('emptyDataList', emptyDataList);

      for (let iDate = 0; iDate < data.length; iDate++) {
         const date = data[iDate];
         const accounts = date.Accounts;

         for (let iAccount = 0; iAccount < accounts.length; iAccount++) {
            const account = accounts[iAccount];

            let seriesIndex = -1;
            let seriesItem: any = null; //Highcharts.SeriesAreaOptions

            if (!seriesHash.hasOwnProperty(account.AccountText)) {

               seriesIndex = seriesList.length;
               seriesHash[account.AccountText] = seriesIndex;

               seriesItem = {
                  name: account.AccountText,
                  type: 'area',
                  yAxis: 0,
                  color: this.service.Colors.GetAccountColor(account.AccountID),
                  data: Object.assign([], emptyDataList)
               };
               console.log('seriesItem', { date, account, seriesItem });
               seriesList.push(seriesItem);

            }

            seriesIndex = seriesHash[account.AccountText];
            seriesItem = seriesList[seriesIndex];

            let dataItem = seriesItem.data[iDate];
            dataItem.y = account.Percent;
            dataItem.GainValue = account.Gain;

         }

      }

      console.log('seriesList', { seriesList });
      return seriesList;
   }

}
