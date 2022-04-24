import { Injectable } from '@angular/core';
import { TranslationService } from 'src/app/shared/translation/translation.service';
import { MonthlyTargetVM } from '../analytics.viewmodels';

import * as Highcharts from 'highcharts';
import { AnalyticsService } from '../analytics.service';
import { enCategoryType } from '../../categories/categories.viewmodels';
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

   constructor(private translation: TranslationService, private service: AnalyticsService) {
      this.BalanceColor = this.service.Colors.GetForecolorSchemeSensitive();
   }

   private BalanceColor = null;
   private IncomeColor = '#4CAF50';
   private ExpenseColor = '#f44336';
   private GoalColor = 'green';

   public async show(data: MonthlyTargetVM) {
      try {
         const options = await this.options(data);
         Highcharts.chart('monthlyTargetContainer', options);
      }
      catch (ex) { console.error(ex); }
   }

   private async options(data: MonthlyTargetVM): Promise<Highcharts.Options> {
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

   private xAxisOptions(data: MonthlyTargetVM): Highcharts.XAxisOptions {
      const categoryList = data.Headers
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

   private async yAxisOptions(data: MonthlyTargetVM): Promise<Highcharts.YAxisOptions[]> {
      const targetText = await this.translation.getValue("ANALYTICS_MONTHLY_TARGET_GOAL_LABEL");
      const targetValue = data.Headers
         .map(x => x.TargetValue)
         .find(x => true);
      const maxBalance = data.Headers
         .map(x => x.BalanceValue)
         .sort((a, b) => a < b ? 1 : -1)
         .reduce((a, b) => a || b, 0) || 0;
      let maxValue = data.Items
         .map(x => x.Value)
         .sort((a, b) => a < b ? 1 : -1)
         .reduce((a, b) => a || b, 0) || 0;
      maxValue = maxValue < 105 ? 105 : maxValue;

      return [{
         title: { text: null },
         gridLineColor: 'transparent',
         // tickPositions: [0, 100, maxValue],
         // max: maxValue,
         plotLines: [{
            value: targetValue,
            color: this.GoalColor,
            label: {
               text: `${targetText}: ${this.translation.getNumberFormat(targetValue, 2)}`,
               x: 0,
               style: { fontSize: '1.3vh', color: this.BalanceColor }
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
         useHTML: true,
         formatter: function () {
            const tootipList = this.points
               .filter(p => (p.point.options as any).realValue != 0)
               .map(p => `
                  <div>
                     <span style="color:${p.color}">\u25CF</span>
                     <span>${p.series.name}:</span>
                     <strong>${self.translation.getNumberFormat((p.point.options as any).realValue, 2)}</strong>
                  </div>
                  `);
            const tootip = tootipList.join('');
            const tootipHeader = `<div><strong>${this.points[0].key}</strong></div>`;
            return `${tootipHeader}${tootip}`;
         }
      };
   }

   private async seriesOptions(data: MonthlyTargetVM): Promise<Highcharts.SeriesOptionsType[]> {
      const result: Highcharts.SeriesOptionsType[] = [];

      const balanceSeries: Highcharts.SeriesOptionsType = {
         name: await this.translation.getValue('ANALYTICS_MONTHLY_TARGET_BALANCE_LABEL'),
         type: 'line',
         yAxis: 1,
         color: this.BalanceColor,
         lineWidth: 1,
         marker: { enabled: true, radius: 2 },
         data: data.Headers
            .sort((a, b) => a.Date > b.Date ? 1 : -1)
            .map(x => ({
               name: x.DateText,
               y: x.BalanceValue,
               realValue: x.BalanceValue
            })),
         zIndex: 10
      };
      result.push(balanceSeries);

      result.push(...this.seriesOptions_getSeriesOptionsType(data, enCategoryType.Income));
      result.push(...this.seriesOptions_getSeriesOptionsType(data, enCategoryType.Expense));

      return result;
   }

   private seriesOptions_getSeriesOptionsType(data: MonthlyTargetVM, type: enCategoryType): Highcharts.SeriesOptionsType[] {

      const filteredData = data.Items
         .filter(x => x.Type == type)
         .sort((a, b) => a.Value < b.Value ? 1 : -1);
      const seriesData = groupBy(filteredData, item => item.SerieText);

      let seriesColor = type == enCategoryType.Income ? this.IncomeColor : this.ExpenseColor;
      const getColor = (): string => {
         const resultColor = seriesColor;
         const rgbToHex = (r: number, g: number, b: number) => '#' + [r, g, b]
            .map(x => x.toString(16).padStart(2, '0')).join('');
         const brightenColor: any = Highcharts.color(seriesColor).brighten(0.05);
         seriesColor = rgbToHex(brightenColor.rgba[0], brightenColor.rgba[1], brightenColor.rgba[2]);
         return resultColor;
      };

      const seriesList = seriesData
         .map(serie => {
            return {
               name: serie[0],
               color: getColor(),
               values: serie[1]
            };
         });

      return seriesList
         .map(serieData => {
            const serieResult: Highcharts.SeriesOptionsType = {
               name: serieData.name,
               type: 'column',
               stacking: type == enCategoryType.Income ? 'normal' : undefined,
               yAxis: 0,
               color: serieData.color,
               data: serieData.values
                  .sort((a, b) => a.Date > b.Date ? 1 : -1)
                  .map(x => ({
                     name: x.DateText,
                     y: x.Value,
                     realValue: x.Value
                  }))
            };
            return serieResult;
         });

   }

}

function groupBy<T>(list: T[], keyGetter: (item: T) => string): [key: string, values: T[]][] {
   const map = new Map<string, T[]>();
   list.forEach((item) => {
      const key = keyGetter(item);
      const collection = map.get(key);
      if (!collection) {
         map.set(key, [item]);
      } else {
         collection.push(item);
      }
   });
   const result: [key: string, values: T[]][] = [];
   for (let item of map.entries()) {
      result.push([item[0], item[1]]);
   }
   return result;
}
