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
