function GetTranslation(key) {
   var result = key;
   GetDataJson({
      url: GetActionUrl("Home", "Translation") + "/" + key + "",
      checkUserToken: false,
      success: function (response) { result = response; },
      async: false
   })
   return result;
};

function dateFromJson(value)
{ return new Date(parseInt(value.substr(6))); }

function dateFormated(value) {
   var dd = value.getDate()
   if (dd < 10) dd = '0' + dd
   var mm = value.getMonth() + 1
   if (mm < 10) mm = '0' + mm
   var yy = value.getFullYear()
   // var yy = value.getFullYear() % 100
   // if (yy < 10) yy = '0' + yy
   return dd + '/' + mm + '/' + yy
}

function dateFormat() {
   return moment("3333-11-22").format("L")
      .replace("3333", "YYYY")
      .replace("11", "MM")
      .replace("22", "DD");
}

function decimalFormated(value) {
   // return Number(value).toFixed(2).replace(".", baseMaskDecimalSeparator);
   return Number(value).toLocaleString(navigator.language, { minimumFractionDigits: 2 });
}

function decimalSeparator() {
   var formatedValue = decimalFormated(1.1);
   return formatedValue.substring(1, 2);
}

function decimalWithouThousandSeparator(formatedValue) {
   if (decimalSeparator() == ',') {
      formatedValue = formatedValue.replace(/[.]/g, '');
   }
   else {
      formatedValue = formatedValue.replace(/[,]/g, '');
   }
   return formatedValue;
}

function decimalParse(formatedValue) {
   if (decimalSeparator() == ',') {
      formatedValue = formatedValue.replace(/[.]/g, '');
      formatedValue = formatedValue.replace(',', '.');
   }
   else {
      formatedValue = formatedValue.replace(/[,]/g, '');
   }
   return parseFloat(formatedValue);
}
