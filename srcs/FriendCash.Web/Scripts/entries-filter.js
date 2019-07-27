var koEntryFilterContext = function (initialYear, initialMonth, initialAccount) {
   var self = this;


   /* DATA */
   self.data = ko.observable();
   self.loadData = function (year, month, account) {
      PostDataJson({
         url: GetActionUrl("Entries", "GetInterval"),
         data: { Year: year, Month: month, Account: account },
         success: function (param) {
            if (param.Result) {
               self.data(new koEntryFilterModel(param.Data));
               $(".dropdown-button").dropdown({ constrainWidth: false });
               self.notifyRefresh();
            }
         },
         done: function () { }
      });
   }


   /* CURRENT FILTER */
   self.currentFilterYear = function () { return self.data().FilterCurrentMonth().Year(); }
   self.currentFilterMonth = function () { return self.data().FilterCurrentMonth().Month(); }
   self.currentFilterAccount = function () { return self.data().FilterCurrentAccount().ID(); }
   self.currentFilter = function () {
      return {
         year: self.currentFilterYear(),
         month: self.currentFilterMonth(),
         account: self.currentFilterAccount()
      };
   };


   /* REFRESH NOTIFY */
   self.notifyControl = ko.observable(0);
   self.notifyRefresh = function () {
      self.notifyControl(self.notifyControl() + 1);
   };
   self.notifyDelayed = ko
      .pureComputed(self.notifyControl)
      .extend({ rateLimit: { method: "notifyWhenChangesStop", timeout: 1000 } });
   self.notifyDelayed
      .subscribe(function (val) {
         self.refreshData();
      }, self);


   /* REFRESH DATA */
   self.refreshData = function () {
      var refreshContext = ko.dataFor(document.getElementById("koEntry"));
      if (refreshContext == null) {
         var timer = setInterval(function () {
            clearInterval(timer);
            self.refreshData();
         }, 500);
      }
      else { refreshContext.loadData(self.currentFilter()); }
   };


   /* CLICK */
   self.monthClick = function (pData) {
      self.loadData(pData.Year, pData.Month, self.currentFilterAccount());
   }
   self.accountClick = function (pData) {
      self.loadData(self.currentFilterYear(), self.currentFilterMonth(), pData.ID);
   }


   /* STARTUP EVENT */
   self.loadData(initialYear, initialMonth, initialAccount);
};

function koEntryFilterModel(value, filterContext) {
   var self = this;
   self.FilterPreviousMonth = ko.observable(new koEntryFilterMonthModel(value.PreviousMonth, filterContext));
   self.FilterCurrentMonth = ko.observable(new koEntryFilterMonthModel(value.CurrentMonth, filterContext));
   self.FilterNextMonth = ko.observable(new koEntryFilterMonthModel(value.NextMonth, filterContext));

   self.FilterCurrentAccount = ko.observable(new koEntryFilterAccountModel(value.CurrentAccount));
   self.FilterAccountList = ko.observableArray();
   $.each(value.AccountList, function (k, v) { self.FilterAccountList.push(new koEntryFilterAccountModel(v)); });
};

function koEntryFilterMonthModel(value, filterContext) {
   var self = this;
   self.Text = ko.observable(value.Text);
   self.Year = ko.observable(value.Year);
   self.Month = ko.observable(value.Month);

   self.monthClick = function (pData) {
      filterContext.monthClick(pData);
   }

};

function koEntryFilterAccountModel(value) {
   var self = this;
   self.ID = ko.observable(value.ID);
   self.Text = ko.observable(value.textValue);
};
