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
