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
