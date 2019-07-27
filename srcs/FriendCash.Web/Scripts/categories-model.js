function koCategoryModel(value) {
   var self = this;
   self.idCategory = ko.observable(value.idCategory);
   self.Text = ko.observable(value.Text);
   self.Type = ko.observable(value.Type);
   self.idParentRow = ko.observable(value.idParentRow);
   self.Childs = ko.observableArray([]);
   if (value.Childs != null) {
      $.each(value.Childs, function (eKey, eValue) {
         self.Childs.push(new koCategoryModel(eValue));
      });
   }
};