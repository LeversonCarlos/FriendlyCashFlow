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
