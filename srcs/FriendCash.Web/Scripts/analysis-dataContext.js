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
