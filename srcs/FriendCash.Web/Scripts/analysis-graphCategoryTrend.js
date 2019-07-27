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
