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
