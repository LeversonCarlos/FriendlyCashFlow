ko.bindingHandlers.notifyer = {
   init: function (element, valueAccessor, allBindingsAccessor, data, context) {
   },
   update: function (element, valueAccessor, allBindingsAccessor, data, context) {
      var unwrapAccessor = ko.utils.unwrapObservable(valueAccessor());  // unwrap to get subscription
      unwrapAccessor(element);
   }
};