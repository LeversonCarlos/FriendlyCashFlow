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

   public async options() {
      return {
         chart: this.options_chart(),
         title: await this.options_title(),
         plotOptions: this.options_plotOptions(),
         xAxis: this.options_xAxis(),
         yAxis: await this.options_yAxis(),
         series: this.options_series(),
         tooltip: this.options_tooltip(),
         credits: { enabled: false },
         legend: { enabled: false },
      };
   }

   private options_chart() {
      return {
         type: 'column',
         backgroundColor: 'transparent'
      };
   }

   private async options_title() {
      return {
         text: await this.translation.getValue("ANALYTICS_CATEGORY_GOALS_TITLE"),
         align: 'left'
      };
   }

   private options_plotOptions(): any {
      return {
         series: {
            stacking: 'normal',
            borderWidth: 0
         }
      };
   }

   private options_xAxis(): any {
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

   private async options_yAxis() {
      return {
         title: { enabled: false },
         plotLines: [{
            value: 100,
            color: 'green',
            label: {
               text: await this.translation.getValue("ANALYTICS_CATEGORY_GOALS_GOAL_LABEL"),
               x: 0,
               style: { fontSize: '1.3vh' }
            },
            zIndex: 7,
            width: 2
         }],
         labels: { enabled: false }
      };
   }

   private options_tooltip(): any {
      return {
         shared: true
      };
   }

   private options_series(): any {
      return [{
         name: ' ',
         color: '#fff'
      }, {
         name: ' ',
         color: '#fff'
      }];
   }

}
