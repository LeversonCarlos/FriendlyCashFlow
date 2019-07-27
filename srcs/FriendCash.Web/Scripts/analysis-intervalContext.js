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
