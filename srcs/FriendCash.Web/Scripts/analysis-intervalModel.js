function koAnalysisIntervalModel(value) {
   var self = this;
   self.PreviousMonth = ko.observable(new koAnalysisIntervalItemModel(value.PreviousMonth));
   self.CurrentMonth = ko.observable(new koAnalysisIntervalItemModel(value.CurrentMonth));
   self.NextMonth = ko.observable(new koAnalysisIntervalItemModel(value.NextMonth));
};

function koAnalysisIntervalItemModel(value) {
   var self = this;
   self.Text = ko.observable(value.Text);
   self.Year = ko.observable(value.Year);
   self.Month = ko.observable(value.Month);
   self.InitialDate = ko.observable(moment(value.InitialDate));
   self.FinalDate = ko.observable(moment(value.FinalDate));
};
