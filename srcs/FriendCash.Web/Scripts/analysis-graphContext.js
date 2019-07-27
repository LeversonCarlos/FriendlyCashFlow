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
