ko.components.register('related-editor', {

   viewModel: function (params) {
      var self = this;
      self.inputFocus = ko.observable(false);
      self.inputName = ko.observable(params && params.inputName || '');
      self.inputLabel = $('label[for=' + self.inputName() + ']');
      self.dropdownName = ko.computed(function () { return self.inputName() + '_DropDown'; });
      self.relatedController = ko.observable(params && params.relatedController || '');
      self.relatedAction = ko.observable(params && params.relatedAction || '');
      self.relatedFilter = ko.observable(params && params.relatedFilter || '');
      self.relatedFilter.subscribe(function () { self.relatedRefresh(); });

      /* INPUT VALUE */
      self.inputValue = params.inputValue;
      self.inputValue.subscribe(function (v) {
         if (params.inputData != undefined && params.inputData != null) {
            params.inputData(ko.toJS(self.inputData));
         }
      });
      self.inputData = ko.observable();

      /* INPUT TEXT */
      // self.inputLoaded = ko.observable(false);
      self.inputText = ko.observable('');
      self.inputTextDelayed = ko
         .pureComputed(self.inputText)
         .extend({ rateLimit: { method: "notifyWhenChangesStop", timeout: 500 } });
      self.inputTextDelayed
         .subscribe(function (val) {
            // if (self.inputLoaded() == true)
            {
               self.inputData(null);
               if (val == '') { self.inputValue(0); }
               self.relatedExecute(0, val);
            }
            // self.inputLoaded(true);
         }, self);

      /* DATA LIST */
      self.relatedData = ko.observableArray([]);
      self.relatedExecute = function (id, text, successCallback) {
         self.relatedData.removeAll();
         GetDataJson({
            url: self.relatedExecute_URL(id, text),
            success: function (relatedRESPONSE)
            {
               if (relatedRESPONSE.Result) {
                  $.each(relatedRESPONSE.Data, function (key, value) {
                     self.relatedData.push(value);
                  });
                  if (successCallback != undefined) { successCallback(); }
               }
            }
         });
      };
      self.relatedExecute_URL = function (id, text) {
         var urlBase = GetActionUrl(self.relatedController(), self.relatedAction());
         var urlParam = "";
         if (id != null && id != 0) { urlParam += "id=" + id + "&"; }
         if (text != null && text != '') { urlParam += "search=" + encodeURI(text) + "&"; }
         if (self.relatedFilter() != '') { urlParam += "filter=" + encodeURI(self.relatedFilter()) + "&"; }
         return urlBase + "?" + urlParam;
      }

      /* OPEN */
      self.relatedOpen = function () {
         self.inputFocus(true);
         $("#" + self.inputName()).dropdown({
            constrainWidth: true,
            belowOrigin: true
         });
      };

      /* SELECT */
      self.relatedSelect = function (item) {
         self.inputData(item);
         self.inputText(item.textValue);
         self.inputValue(item.ID);
         self.inputLabel.addClass("active");
      };
      self.inputRefreshed = function () { };

      /* CLOSE */
      self.relatedClose = function () {
         self.inputFocus(false);
      };

      /* REFRESH */
      self.relatedRefresh = function () {
         if (self.inputValue() == '' || self.inputValue() == 0)
         { self.relatedExecute(0, ''); }
         else
         {
            self.relatedExecute(self.inputValue(), '', function () {
               if (self.relatedData().length == 1) {
                  self.relatedSelect(self.relatedData()[0]);                  
               }
            });
         }
      };

      /* STARTUP EVENT */
      self.relatedRefresh();

   },

   template: '' + 
      '<input ' + 
         'type="text" ' +
         'class="form-control" ' +
         'data-bind="textInput: inputText, ' + 
            'attr:{id:inputName, name:inputName, ' +
               '\'data-related-controller\':relatedController, ' +
               '\'data-related-action\':relatedAction, ' +
               '\'data-related-filter\':relatedFilter, ' +
               '\'data-activates\':dropdownName}, ' +
            'event:{focus:relatedOpen,blur:relatedClose} "' +
         '/> ' +
      '<ul class="dropdown-content" data-bind="attr:{id:dropdownName},foreach:relatedData ">' +
         '<li>' + 
            '<a href="#!" data-bind="html:htmlValue,click:$parent.relatedSelect">' +
            '</a>' + 
         '</li>' +
      '</ul>' +
      ''

});