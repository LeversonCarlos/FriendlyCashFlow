var koAnalysisIntervalContext = function (initialYear, initialMonth) {
   var self = this;


   /* DATA */
   self.data = ko.observable();
   self.loadData = function (year, month) {
      PostDataJson({
         url: GetActionUrl("Analysis", "GetInterval"),
         data: { Year: year, Month: month },
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result) {
               self.data(new koAnalysisIntervalModel(actionBUNDLE.Data));
               var currentMonth = ko.toJS(self.data().CurrentMonth());
               self.notifyRefresh(currentMonth);
            }
         },
         done: function () { }
      });
   }


   /* REFRESH NOTIFY */
   self.notifyParams = null;
   self.notifyControl = ko.observable(0);
   self.notifyRefresh = function (currentMonth) {
      self.notifyParams = currentMonth;
      self.notifyControl(self.notifyControl() + 1);
   };
   self.notifyDelayed = ko
      .pureComputed(self.notifyControl)
      .extend({ rateLimit: { method: "notifyWhenChangesStop", timeout: 1000 } });
   self.notifyDelayed
      .subscribe(function (val) {
         self.refreshData(self.notifyParams);
      }, self);


   /* REFRESH DATA */
   self.refreshData = function (currentMonth) {
      var refreshContext = ko.dataFor(document.getElementById("koAnalysisData"));
      if (refreshContext == null) {
         var timer = setInterval(function () {
            clearInterval(timer);
            self.refreshData(currentMonth);
         }, 500);
      }
      else { refreshContext.loadData(currentMonth); }
   };


   /* CLICK */
   self.click = function (pData) {
      self.loadData(pData.Year, pData.Month);
   }


   /* OPTIONS */
   self.optionsActive = ko.observable(false);
   self.optionsClick = function () {
      self.optionsActive(!self.optionsActive());

      if (self.optionsActive()) {
         $("#mainAnalysisGraph").addClass('l8').addClass('pull-l4');
         $("#mainAnalysisData").removeClass('hide').addClass('l4').addClass('push-l8');
      }
      else {
         $("#mainAnalysisGraph").removeClass('l8').removeClass('pull-l4');
         $("#mainAnalysisData").addClass('hide').removeClass('l4').removeClass('push-l8');
      }
      window.dispatchEvent(new Event('resize'));

   };


   /* STARTUP EVENT */
   self.loadData(initialYear, initialMonth);
};

function koAnalysisIntervalModel(value) {
   var self = this;
   self.PreviousMonth = ko.observable(new koAnalysisIntervalItemModel(value.PreviousMonth));
   self.CurrentMonth = ko.observable(new koAnalysisIntervalItemModel(value.CurrentMonth));
   self.NextMonth = ko.observable(new koAnalysisIntervalItemModel(value.NextMonth));
};

function koAnalysisIntervalItemModel(value) {
   var self = this;
   self.Text = ko.observable(value.Text);
   self.Year = ko.observable(value.Year);
   self.Month = ko.observable(value.Month);
   self.InitialDate = ko.observable(moment(value.InitialDate));
   self.FinalDate = ko.observable(moment(value.FinalDate));
};

var koAnalysisDataContext = function () {
   var self = this;


   /* LOADING */
   self.isLoading = function (val) {
      self.graphContext(function (context) {
         context.isLoading(val);
      });
   };

   /* DATA */
   self.data = ko.observable();
   self.loadData = function (currentMonth) {
      self.isLoading(true);
      PostDataJson({
         url: GetActionUrl("Analysis", "GetData"),
         data: { Year: currentMonth.Year, Month: currentMonth.Month },
         success: function (actionBUNDLE) {            
            if (actionBUNDLE.Result) {
               self.data(new koAnalysisDataModel(actionBUNDLE.Data, currentMonth, self));
               self.limitHeight();
               self.refreshData();
            }
         },
         error: function () { self.isLoading(false); }
      });
   }


   /* LIMIT HEIGHT */
   self.limitHeight = function () {

      // DEFINE HEIGHT
      var initialTop = $("#koAnalysisData").offset().top;
      var finalTop = $("footer").offset().top;
      var headerHeight = $("#koAnalysisData .collapsible-header").height();
      var maxHeight = (finalTop - initialTop) - (headerHeight * 6.5);

      // VALIDATE MINIMUM HEIGHT
      var minHeight = (Math.abs($("#koAnalysisData .collection-item").height()) * 6);
      if (maxHeight < minHeight) { maxHeight = minHeight; }

      // APPLY
      $("#koAnalysisData .collapsible-body")
         .css("max-height", maxHeight)
         .css("overflow-y", "scroll");

   }


   /* REFRESH DATA */
   self.refreshData = function () {
      self.isLoading(true);
      var originalData = JSON.parse(ko.toJSON(self.data));
      PostDataJson({
         url: GetActionUrl("Analysis", "GetGraphs"),
         data: originalData,
         success: function (actionBUNDLE) {
            if (actionBUNDLE.Result) {
               actionBUNDLE.Data.data.currentMonth = ko.toJS(self.data().currentMonth);
               self.graphContext(function (context) {
                  context.loadData(actionBUNDLE.Data);
                  context.isLoading(false);
               });
            }
         },
         error: function () { self.isLoading(false); }
      });
   };


   /* GRAPH CONTEXT */
   self.graphContext = function (executeCallback) {
      var context = ko.dataFor(document.getElementById("koAnalysisGraph"));
      if (context == null) {
         var timer = setInterval(function () {
            clearInterval(timer);
            self.graphContext(executeCallback);
         }, 500);
      }
      else { executeCallback(context); }
   };


};

function koAnalysisDataModel(value, currentMonth, dataContext) {
   var self = this;
   self.currentMonth = ko.observable(currentMonth);

   // DIMENSIONS
   self.Dimensions = ko.observable(new koAnalysisDataDimensionsModel(value, self));

   // FILTER
   self.filterControl = ko.observable(0);
   self.filterNotify = function () { self.filterControl(self.filterControl() + 1); };
   self.filterDelayed = ko
      .pureComputed(self.filterControl)
      .extend({ rateLimit: { method: "notifyWhenChangesStop", timeout: 1000 } });
   self.filterDelayed
      .subscribe(function (val) { dataContext.refreshData(); }, self);

};

function koAnalysisDataDimensionsModel(value, dataHeader) {
   var self = this;

   // ACCOUNTS DIMENSION
   self.Accounts = ko.observableArray();
   $.each(value.Dimensions.Accounts, function (k, v) { self.Accounts.push(new koAnalysisDataDimensionModel(v, dataHeader)); });

   // CATEGORIES DIMENSION
   self.Categories = ko.observableArray();
   $.each(value.Dimensions.Categories, function (k, v) { self.Categories.push(new koAnalysisDataDimensionModel(v, dataHeader)); });

   // PATTERNS DIMENSION
   self.Patterns = ko.observableArray();
   $.each(value.Dimensions.Patterns, function (k, v) { self.Patterns.push(new koAnalysisDataDimensionModel(v, dataHeader)); });

   // PLANNED DIMENSION
   self.Planned = ko.observableArray();
   $.each(value.Dimensions.Planned, function (k, v) { self.Planned.push(new koAnalysisDataDimensionModel(v, dataHeader)); });

   // PAID DIMENSION
   self.Paid = ko.observableArray();
   $.each(value.Dimensions.Paid, function (k, v) { self.Paid.push(new koAnalysisDataDimensionModel(v, dataHeader)); });

};

function koAnalysisDataDimensionModel(value, dataHeader) {
   var self = this;
   self.ID = ko.observable(value.ID);
   self.ParentText = ko.observable(value.ParentText);
   self.Text = ko.observable(value.Text);
   self.Type = ko.observable(value.Type);
   self.Selected = ko.observable(value.Selected);
   self.clicked = function () {
      self.Selected(!self.Selected());
      dataHeader.filterNotify();
   }
};

var koAnalysisGraphContext = function () {
   var self = this;
   self.isLoading = ko.observable(false);

   /* COLORS */
   self.highColors = ['#7cb5ec', '#90ed7d', '#f7a35c', '#8085e9', '#f15c80', '#e4d354', '#2b908f', '#f45b5b', '#91e8e1', '#434348'];
   self.colors = ['#64b5f6', '#aed581', '#ff8a65', '#7986cb', '#f06292', '#ffd54f', '#4db6ac', '#e57373', '#4dd0e1', '#455a64'];
   self.redColor = '#c62828';
   self.greenColor = '#2e7d32';
   self.blueColor = '#1565c0 ';

   /* PROPERTIES */
   self.data = ko.observable();
   self.dailyFlow = ko.observable();
   self.categoryExpenseDetails = ko.observable();
   self.categoryIncomeDetails = ko.observable();
   self.categoryGoal = ko.observable();
   self.categoryTrend = ko.observable();
   self.balanceTrend = ko.observable();
   self.cumulatedEntries = ko.observable();

   /* LOAD */
   self.loadData = function (graphData) {
      self.data(graphData.data);
      self.dailyFlow(graphData.dailyFlow);
      self.categoryExpenseDetails(graphData.categoryExpenseDetails);
      self.categoryIncomeDetails(graphData.categoryIncomeDetails);
      self.categoryGoal(graphData.categoryGoal);
      self.categoryTrend(graphData.categoryTrend);
      self.balanceTrend(graphData.balanceTrend);
      self.cumulatedEntries(graphData.cumulatedEntries);
   };

};

function graphDailyFlow(element, dataContext) {
   var graphDailyFlowInstance = new graphDailyFlowObject(element);
   graphDailyFlowInstance.Initialize();
   graphDailyFlowInstance.Update(dataContext);
};

var graphDailyFlowObject = function (element) {
   var self = this;


   /* PROPERTIES */
   self.graphObject = null;
   self.graphProperties = null;
   self.container = $(element.previousElementSibling);


   /* INITIALIZE */
   self.Initialize = function () {

      // GRAPH PROPERTIES
      self.graphProperties = {
         chart: {
            type: 'area',
            renderTo: self.container[0],
            /*margin: [0, 0, 0, 0],*/
            backgroundColor: 'transparent'
         },
         title: {
            text: titleDailyFlow,
            align: 'left'
         },
         credits: { enabled: false },
         xAxis: {
            categories: [],
            tickmarkPlacement: 'on',
            showFirstLabel: false,
            title: { enabled: false },
            labels: { enabled: false },
            tickLength: 1
         },
         yAxis: {
            min: 0,
            title: { enabled: false },
            labels: {
               enabled: true,
               formatter: function () {
                  return Number(this.value).toLocaleString(navigator.language, { minimumFractionDigits: 0 });
               }
            }
         },
         legend: {
            verticalAlign: 'bottom',
            symbolHeight: 8, 
            enabled: true
      },
         tooltip: {
            shared: true,
            formatter: function () {
               if (this.x == 0) { return false; }
               var tooltipResult = '<strong>' + this.x + '</strong>';
               $.each(this.points, function () {
                  tooltipResult +=
                     '<br/>' +
                     '<span style="color:' + this.color + '">\u25CF</span> ' +
                     '<span>' + this.series.name + '</span>: ' +
                     '<strong>' + decimalFormated(this.y) + '</strong>';
               });
               return tooltipResult;
            }
         },
         plotOptions: {
            area: {
               marker: { symbol: 'circle', radius: 2 },
               stacking: 'normal'
            }
         },
         series: [{
            name: ' ', 
            color:'#fff'
         }, {
            name: ' ',
            color: '#fff'
         }, {
            name: ' ',
            color: '#fff'
         }]
      };

      self.graphObject = new Highcharts.Chart(self.graphProperties);

   };

   // UPDATE 
   self.Update = function (dataContext) {
      var data = dataContext.dailyFlow();
      if (data == undefined) { return; }

      // CATEGORIES
      self.graphObject.xAxis[0].update({ categories: data.category });

      // SERIES
      data.unpaid.color = Highcharts.Color(dataContext.redColor).brighten(0.5).get();
      self.graphObject.series[0].update(data.unpaid);

      // UNPLANNED
      data.unplanned.color = Highcharts.Color(dataContext.redColor).brighten(0.2).get();
      self.graphObject.series[1].update(data.unplanned);

      // PAID
      data.paid.color = dataContext.redColor;
      self.graphObject.series[2].update(data.paid);

      // TODAY
      var xPlotLines = null;
      if (data.currentDay > 0) {
         xPlotLines = [{
            value: data.currentDay,
            color: dataContext.redColor,
            label: { text: labelDailyFlowToday, rotation: -90, align: 'right', x: -4, y: 0, style: {fontSize:'1.3vh'} },
            zIndex: 10,
            width: 1
         }];
      }
      self.graphObject.xAxis[0].update({ plotLines: xPlotLines });

   };

};

$('.daily-flow-expandable-button').click(function (e) {
   var self = $(e.target);
   var card = self.parents(".card");

   // RESIZE CARD
   if (card.hasClass("expanded"))
   { card.removeClass("expanded"); }
   else
   { card.addClass("expanded"); }

   // REFRESH DATA
   var elementArray = card.find("code");
   var element = elementArray[0];
   var data = ko.dataFor(element);
   graphDailyFlow(element, data);

});

function graphCategoryDetails(element, dataContext, categoryType) {
   var graphCategoryDetailsInstance = new graphCategoryDetailsObject(element, categoryType);
   graphCategoryDetailsInstance.Initialize();
   graphCategoryDetailsInstance.Update(dataContext);
};

var graphCategoryDetailsObject = function (element, categoryType) {
   var self = this;


   /* PROPERTIES */
   self.graphObject = null;
   self.graphProperties = null;
   self.container = $(element.previousElementSibling);


   /* INITIALIZE */
   self.Initialize = function () {

      // GRAPH PROPERTIES
      self.graphProperties = {
         chart: {
            type: 'pie',
            renderTo: self.container[0],
            /*margin: [0, 0, 0, 0],*/
            events: {
               drilldown: function (e) {
                  // console.log("drilldown", e);
                  // this.subtitle.update({ text: e.point.name });
                  this.addSeriesAsDrilldown(e.point, { data: e.point.options.drillData });
               },
               drillup: function (e) {
                  // console.log("drillup", e);
               }
            },
            backgroundColor: 'transparent'
         },
         title: {
            text: (categoryType == enCategoryTypeExpense ? titleCategoryExpenseDetails : titleCategoryIncomeDetails),
            align: 'left'
         },
         /*
         subtitle: {
            text: '',
            align: 'left'
         },
         */
         credits: { enabled: false },
         legend: { enabled: false },
         plotOptions: {
            series: {
               borderWidth: 0,
               dataLabels: {
                  enabled: true,
                  distance: -5,
                  formatter: function () {
                     return this.percentage > 2 ? this.point.name : null;
                  }
               }
            }
         },
         tooltip: {
            formatter: function () {
               return '<span style="color:' + this.color + '">\u25CF</span> ' +
                      '<span>' + this.point.name + '</span>: ' +
                      '<strong>' + decimalFormated(this.point.y) + '</strong>' +
                      '';
            }
         },
         series: [{
            name: 'MAIN', 
            data: []
         }],
         drilldown: {
            drillUpButton: {
               relativeTo: 'spacingBox',
               position: {
                  align:'left',
                  y: 25,
                  x: 0
               }
            },
            series: []
         }
      };

      Highcharts.setOptions({
         lang: {
            drillUpText: '<<'
         }
      });
      self.graphObject = new Highcharts.Chart(self.graphProperties);

   };

   // UPDATE 
   self.Update = function (dataContext) {
      var colors = dataContext.colors;
      var data = null;
      if (categoryType == enCategoryTypeExpense) { data = dataContext.categoryExpenseDetails(); }
      else if (categoryType == enCategoryTypeIncome) { data = dataContext.categoryIncomeDetails(); }
      if (data == undefined) { return; }

      // LEVEL1
      for (var level1Index = 0; level1Index < data.seriesData.length; level1Index++) {
         var level1Model = data.seriesData[level1Index];
         level1Model.color = colors[level1Model.colorIndex];

         // LEVEL2
         for (var level2Index = 0; level2Index < level1Model.drillData.length; level2Index++) {
            var level2Model = level1Model.drillData[level2Index];
            level2Model.color = Highcharts.Color(level1Model.color).brighten(level2Model.colorBrightness).get();

            // LEVEL3
            for (var level3Index = 0; level3Index < level2Model.drillData.length; level3Index++) {
               var level3Model = level2Model.drillData[level3Index];
               level3Model.color = Highcharts.Color(level2Model.color).brighten(level3Model.colorBrightness).get();
            };

            // LEVEL 4
            for (var level4Index = 0; level4Index < level3Model.drillData.length; level4Index++) {
               var level4Model = level3Model.drillData[level4Index];
               level4Model.color = Highcharts.Color(level3Model.color).brighten(level4Model.colorBrightness).get();
            }

         };

      };

      // APPLY
      self.graphObject.series[0].update({ data: data.seriesData }, false);
      self.graphObject.redraw();

   };

};

$('.category-details-expandable-button').click(function (e) {
   var self = $(e.target);
   var card = self.parents(".card");

   // RESIZE CARD
   if (card.hasClass("expanded"))
   { card.removeClass("expanded"); }
   else
   { card.addClass("expanded"); }

   // REFRESH DATA
   var elementArray = card.find("code");
   var element = elementArray[0];
   var data = ko.dataFor(element);
   var additionalParam = card.attr("data-additional-param");
   graphCategoryDetails(element, data, additionalParam);

});

function graphCategoryGoal(element, dataContext) {
   var graphCategoryGoalInstance = new graphCategoryGoalObject(element);
   graphCategoryGoalInstance.Initialize();
   graphCategoryGoalInstance.Update(dataContext);
};

var graphCategoryGoalObject = function (element) {
   var self = this;


   /* PROPERTIES */
   self.graphObject = null;
   self.graphProperties = null;
   self.container = $(element.previousElementSibling);


   /* INITIALIZE */
   self.Initialize = function () {

      // GRAPH PROPERTIES
      self.graphProperties = {
         chart: {
            type: 'column',
            renderTo: self.container[0],
            /*margin: [0, 0, 0, 0],*/
            backgroundColor: 'transparent'
         },
         title: {
            text: titleCategoryGoal,
            align: 'left'
         },
         credits: { enabled: false },
         legend: { enabled: false },
         plotOptions: {
            series: {
               stacking: 'normal',
               borderWidth: 0
            }
         },
         xAxis: {
            categories: [],
            title: { enabled: false },
            labels: {
               rotation:-90,
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
         },
         yAxis: {
            title: { enabled: false },
            plotLines: [{
               value: 100,
               color: 'green',
               label: { text: labelCategoryGoal, x: 0, style: { fontSize: '1.3vh' } },
               zIndex:7, 
               width: 2
            }],
            labels: { enabled: false }
         },
         tooltip: {
            shared: true,
            formatter: function () {
               var tooltipResult = '';
               var goalValue = 0;
               $.each(this.points, function () {
                  if (this.point.goalValue > 0) {
                     tooltipResult =
                        '<br/>' +
                        '<span style="color:' + 'green' + '">\u25CF</span> ' +
                        '<span>' + labelCategoryGoal + '</span>: ' +
                        '<strong>' + decimalFormated(this.point.goalValue) + '</strong>' +
                        tooltipResult;
                  }
                  tooltipResult +=
                     '<br/>' +
                     '<span style="color:' + this.color + '">\u25CF</span> ' +
                     '<span>' + this.series.name + '</span>: ' +
                     '<strong>' + decimalFormated(this.point.realValue) + '</strong>';
               });
               tooltipResult = '<strong>' + this.x + '</strong>' + tooltipResult;
               return tooltipResult;
            }
         },
         series: [{
            name: ' ',
            color: '#fff'
         }, {
            name: ' ',
            color: '#fff'
         }]
      };

      self.graphObject = new Highcharts.Chart(self.graphProperties);

   };

   // UPDATE 
   self.Update = function (dataContext) {
      var data = dataContext.categoryGoal();
      var colors = dataContext.colors;
      if (data == undefined) { return; }

      // PAID
      for (var seriesIndex = 0; seriesIndex < data.paidData.data.length; seriesIndex++) {
         var seriesModel = data.paidData.data[seriesIndex];
         seriesModel.color = colors[seriesModel.colorIndex];
      };

      // UNPAID
      for (var seriesIndex = 0; seriesIndex < data.unpaidData.data.length; seriesIndex++) {
         var seriesModel = data.unpaidData.data[seriesIndex];
         seriesModel.color = Highcharts.Color(colors[seriesModel.colorIndex]).brighten(seriesModel.colorBrightness).get();
      };

      // APPLY
      self.graphObject.xAxis[0].update({ categories: data.categoryList });
      self.graphObject.yAxis[0].update({ gridLineColor: 'transparent', tickPositions: [0, 100, data.maxValue], max: data.maxValue });
      self.graphObject.series[0].update(data.unpaidData);
      self.graphObject.series[1].update(data.paidData);

   };

};

$('.category-goal-expandable-button').click(function (e) {
   var self = $(e.target);
   var card = self.parents(".card");

   // RESIZE CARD
   if (card.hasClass("expanded"))
   { card.removeClass("expanded"); }
   else
   { card.addClass("expanded"); }

   // REFRESH DATA
   var elementArray = card.find("code");
   var element = elementArray[0];
   var data = ko.dataFor(element);
   graphCategoryGoal(element, data);

});

function graphCategoryTrend(element, dataContext) {
   var graphCategoryTrendInstance = new graphCategoryTrendObject(element);
   graphCategoryTrendInstance.Initialize();
   graphCategoryTrendInstance.Update(dataContext);
};

var graphCategoryTrendObject = function (element) {
   var self = this;


   /* PROPERTIES */
   self.graphObject = null;
   self.graphProperties = null;
   self.container = $(element.previousElementSibling);


   /* INITIALIZE */
   self.Initialize = function () {

      // GRAPH PROPERTIES
      self.graphProperties = {
         chart: {
            type: 'line',
            renderTo: self.container[0],
            /*margin: [0, 0, 0, 0],*/
            backgroundColor: 'transparent'
         },
         title: {
            text: titleCategoryTrend,
            align: 'left'
         },
         credits: { enabled: false },
         legend: {
            align: 'right',
            verticalAlign: 'middle',
            layout: 'vertical',
            symbolHeight: 8,
            enabled: true
         },
         xAxis: {
            categories: [],
            tickmarkPlacement: 'on',
            title: { enabled: false },
            labels: { enabled: true },
            tickLength: 1
         },
         yAxis: {
            title: { enabled: false },
            labels: {
               enabled: true,
               formatter: function () {
                  return Number(this.value).toLocaleString(navigator.language, { minimumFractionDigits: 0 });
               }
            }
         },
         plotOptions: {
            line: {
               marker: { symbol: 'circle', radius: 2 }
            }
         },
         tooltip: {
            shared: true,
            formatter: function () {

               // INITIALIZE
               var tooltipResult = '<strong>' + this.x + '</strong>';

               // REORDER POINTS
               var points = [];
               for (var i = 0; i < this.points.length; i++) {
                  var point = this.points[i];
                  points.push({ name: point.series.name, value: point.y, color: point.color });
               };
               points.sort(function (a, b) {
                  return parseFloat(b.value) - parseFloat(a.value);
               });

               // RESULT
               $.each(points, function () {
                  tooltipResult +=
                     '<br/>' +
                     '<span style="color:' + this.color + '">\u25CF</span> ' +
                     '<span>' + this.name + '</span>: ' +
                     '<strong>' + decimalFormated(this.value) + '</strong>';
               });
               return tooltipResult;
            }
         },
         series: []
      };

      self.graphObject = new Highcharts.Chart(self.graphProperties);

   };

   // UPDATE 
   self.Update = function (dataContext) {
      var data = dataContext.categoryTrend();
      var colors = dataContext.colors;
      if (data == undefined) { return; }

      // CATEGORIES
      self.graphObject.xAxis[0].update({ categories: data.categoryList });

      // SERIES
      if (self.graphObject.series != undefined) {
         for (var itemIndex = (self.graphObject.series.length - 1) ; itemIndex >= 0; itemIndex--) {
            self.graphObject.series[itemIndex].remove();
         }
      }
      for (var seriesIndex = 0; seriesIndex < data.seriesData.length; seriesIndex++) {
         var seriesModel = data.seriesData[seriesIndex];
         seriesModel.color = colors[seriesModel.colorIndex];
         self.graphObject.addSeries(seriesModel);
      };

      // LIMITS
      // self.graphObject.yAxis[0].update({ min: data.limits.minValue, max: data.limits.maxValue, tickInterval: data.limits.tickInterval });

   };

};

$('.category-trend-expandable-button').click(function (e) {
   var self = $(e.target);
   var card = self.parents(".card");

   // RESIZE CARD
   if (card.hasClass("expanded"))
   { card.removeClass("expanded"); }
   else
   { card.addClass("expanded"); }

   // REFRESH DATA
   var elementArray = card.find("code");
   var element = elementArray[0];
   var data = ko.dataFor(element);
   graphCategoryTrend(element, data);

});

function graphBalanceTrend(element, dataContext) {
   var graphBalanceTrendInstance = new graphBalanceTrendObject(element);
   graphBalanceTrendInstance.Initialize();
   graphBalanceTrendInstance.Update(dataContext);
};

var graphBalanceTrendObject = function (element) {
   var self = this;


   /* PROPERTIES */
   self.graphObject = null;
   self.graphProperties = null;
   self.container = $(element.previousElementSibling);


   /* INITIALIZE */
   self.Initialize = function () {

      // GRAPH PROPERTIES
      self.graphProperties = {
         chart: {
            renderTo: self.container[0],
            /*margin: [0, 0, 0, 0],*/
            backgroundColor: 'transparent'
         },
         title: {
            text: titleBalanceTrend,
            align: 'left'
         },
         credits: { enabled: false },
         legend: {
            align: 'right',
            verticalAlign: 'middle',
            layout: 'vertical',
            symbolHeight: 8,
            enabled: true
         },
         xAxis: {
            categories: [],
            tickmarkPlacement: 'on',
            title: { enabled: false },
            labels: { enabled: true },
            tickLength: 1
         },
         yAxis: [{
            title: { enabled: false },
            labels: {
               enabled: true,
               formatter: function () {
                  return Number(this.value).toLocaleString(navigator.language, { minimumFractionDigits: 0 });
               }
            }
         }, {
            title: { enabled: false },
            labels: { enabled: false }
         }],
         plotOptions: {
            series: {
               stacking: 'normal'
            }
         },
         tooltip: {
            shared: true,
            formatter: function () {
               var tooltipResult = '<strong>' + this.x + '</strong>';
               $.each(this.points, function () {
                  tooltipResult +=
                     '<br/>' +
                     '<span style="color:' + this.color + '">\u25CF</span> ' +
                     '<span>' + this.series.name + '</span>: ' +
                     '<strong>' + decimalFormated(this.y) + '</strong>';
               });
               return tooltipResult;
            }
         },
         series: [{
            name: ' ',
            type: 'column',
            yAxis: 0,
            color: '#fff'
         }, {
            name: ' ',
            type: 'column',
            yAxis: 0,
            color: '#fff'
         }, {
            name: ' ',
            type: 'line',
            yAxis: 1,
            lineWidth: 1,
            marker: { enabled: true, radius: 2 },
            color: '#fff'
         }]
      };

      self.graphObject = new Highcharts.Chart(self.graphProperties);

   };

   // UPDATE 
   self.Update = function (dataContext) {
      var data = dataContext.balanceTrend();
      if (data == undefined) { return; }

      // CATEGORIES
      self.graphObject.xAxis[0].update({ categories: data.categoryList });

      // INCOME
      data.IncomeData.color = Highcharts.Color(dataContext.greenColor).brighten(0.2).get();
      self.graphObject.series[0].update(data.IncomeData);

      // EXPENSE
      data.ExpenseData.color = Highcharts.Color(dataContext.redColor).brighten(0.2).get();
      self.graphObject.series[1].update(data.ExpenseData);

      // EXPENSE
      data.BalanceData.color = Highcharts.Color(dataContext.blueColor).brighten(0.2).get();
      self.graphObject.series[2].update(data.BalanceData);

      // LIMITS
      // self.graphObject.yAxis[0].update({ min: data.limits.minValue, max: data.limits.maxValue, tickInterval: data.limits.tickInterval });
      self.graphObject.yAxis[1].update({ min: data.limits.minValue, max: data.limits.maxValue });

   };

};

$('.balance-trend-expandable-button').click(function (e) {
   var self = $(e.target);
   var card = self.parents(".card");

   // RESIZE CARD
   if (card.hasClass("expanded"))
   { card.removeClass("expanded"); }
   else
   { card.addClass("expanded"); }

   // REFRESH DATA
   var elementArray = card.find("code");
   var element = elementArray[0];
   var data = ko.dataFor(element);
   graphBalanceTrend(element, data);

});

function graphCumulatedEntries(element, dataContext) {
   var graphCumulatedEntriesInstance = new graphCumulatedEntriesObject(element);
   graphCumulatedEntriesInstance.Initialize();
   graphCumulatedEntriesInstance.Update(dataContext);
};

var graphCumulatedEntriesObject = function (element) {
   var self = this;


   /* PROPERTIES */
   self.graphObject = null;
   self.graphProperties = null;
   self.container = $(element.previousElementSibling);


   /* INITIALIZE */
   self.Initialize = function () {

      // GRAPH PROPERTIES
      self.graphProperties = {
         chart: {
            renderTo: self.container[0],
            /*margin: [0, 0, 0, 0],*/
            backgroundColor: 'transparent'
         },
         title: {
            text: titleCumulatedEntries,
            align: 'left'
         },
         credits: { enabled: false },
         legend: { enabled: false },
         xAxis: {
            categories: [],
            title: { enabled: false },
            labels: {
               rotation: -90,
               enabled: true,
               step: 1,
               reserveSpace: false,
               align: 'left',
               y: -2,
               style: {
                  color: "#000",
                  textShadow: '0px 0px 5px white'
               }
            },
            tickLength: 1
         },
         yAxis: [{
            title: { enabled: false },
            labels: {
               enabled: true,
               formatter: function () {
                  return Number(this.value).toLocaleString(navigator.language, { minimumFractionDigits: 0 });
               }
            }
         }, {
            min: 0, max: 100,
            title: { enabled: false },
            gridLineWidth: 0,
            labels: { enabled: false }
         }],
         tooltip: {
            shared:true,
            formatter: function () {
               return '' +
                      '<span style="color:' + this.color + '">' + this.x + '</span>:<br/>' +
                      '<strong>' + decimalFormated(this.y) + '</strong> ' +
                      '(' + 
                      '<span>' + this.points[1].y + '%' + '</span>' +
                      ')' + 
                      '';
            }
         },
         series: [{
            name: 'VALUES',
            type: 'column'
         }, {
            name: 'PARETO',
            type: 'spline',
            yAxis: 1,
            lineWidth: 1,
            marker: { enabled: true, radius: 2 }
         }]
      };

      self.graphObject = new Highcharts.Chart(self.graphProperties);

   };

   // UPDATE 
   self.Update = function (dataContext) {
      var data = dataContext.cumulatedEntries();
      if (data == undefined) { return; }

      // CATEGORIES
      self.graphObject.xAxis[0].update({ categories: data.categoryList });

      // SERIES
      var color = Highcharts.Color(dataContext.blueColor).brighten(0.2).get();
      self.graphObject.series[0].update({ color: color, data: data.data });

      // PARETO
      self.graphObject.series[1].update({ color: dataContext.blueColor, data: data.pareto });

      // LIMITS
      // self.graphObject.yAxis[0].update({ min: data.limits.minValue, max: data.limits.maxValue });

   };

};

$('.cumulated-entries-expandable-button').click(function (e) {
   var self = $(e.target);
   var card = self.parents(".card");

   // RESIZE CARD
   if (card.hasClass("expanded"))
   { card.removeClass("expanded"); }
   else
   { card.addClass("expanded"); }

   // REFRESH DATA
   var elementArray = card.find("code");
   var element = elementArray[0];
   var data = ko.dataFor(element);
   graphCumulatedEntries(element, data);

});