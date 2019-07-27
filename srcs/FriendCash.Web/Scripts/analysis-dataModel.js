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
