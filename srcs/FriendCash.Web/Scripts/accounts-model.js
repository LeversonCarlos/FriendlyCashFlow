function koAccountModel(value) {
   var self = this;
   self.idAccount = ko.observable(value.idAccount);
   self.Text = ko.observable(value.Text);
   self.TypeValue = ko.observable(value.TypeValue);
   self.Type = ko.observable(value.Type);
   self.ClosingDay = ko.observable(value.ClosingDay);
   self.DueDay = ko.observable(value.DueDay);
   self.Active = ko.observable(value.Active);
};