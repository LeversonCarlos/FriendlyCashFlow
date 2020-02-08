import { Injectable } from '@angular/core';
import { TranslationService } from 'src/app/shared/translation/translation.service';

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
export class CategoryGoalsChart {

   constructor(private translation: TranslationService) { }

   public get options(): any {
      return {
         chart: this.options_chart,
         title: this.options_title,
         plotOptions: this.options_plotOptions,
         xAxis: this.options_xAxis,
         yAxis: this.options_yAxis,
         series: this.options_series,
         tooltip: this.options_tooltip,
         credits: { enabled: false },
         legend: { enabled: false },
      };
   }

   private get options_chart(): any {
      return {
         type: 'column',
         backgroundColor: 'transparent'
      };
   }

   private get options_title(): any {
      return {
         text: (async () => await this.translation.getValue("ANALYTICS_CATEGORY_GOALS_TITLE"))(),
         align: 'left'
      };
   }

   private get options_plotOptions(): any {
      return {
         series: {
            stacking: 'normal',
            borderWidth: 0
         }
      };
   }

   private get options_xAxis(): any {
      return {
         categories: [],
         title: { enabled: false },
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

   private get options_yAxis(): any {
      return {
         title: { enabled: false },
         plotLines: [{
            value: 100,
            color: 'green',
            label: {
               text: (async () => await this.translation.getValue("ANALYTICS_CATEGORY_GOALS_GOAL_LABEL")),
               x: 0,
               style: { fontSize: '1.3vh' }
            },
            zIndex: 7,
            width: 2
         }],
         labels: { enabled: false }
      };
   }

   private get options_tooltip(): any {
      return {
         shared: true
      };
   }

   private get options_series(): any {
      return [{
         name: ' ',
         color: '#fff'
      }, {
         name: ' ',
         color: '#fff'
      }];
   }

}
