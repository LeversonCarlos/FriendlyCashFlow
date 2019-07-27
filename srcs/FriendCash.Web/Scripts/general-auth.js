function refreshAuthToken(doneCallback) {
   GetDataJson({
      url: GetActionUrl("Auth", "CheckToken"),
      checkUserToken: false,
      success: function (responseBundle) {
         if (responseBundle.Result) {
            authUserTicketData = getUserTicketData(responseBundle.Data.UserTicket);
            console.log("refreshAuthToken", authUserTicketData);
            authUserTicketData.refreshTimer = setInterval(function () {
               authUserTicketData.IsTokenExpired = true;
               clearInterval(authUserTicketData.refreshTimer);
               refreshAuthToken();
            }, authUserTicketData.refreshSeconds * 1000);
         }
         else { console.log("refreshAuthToken-error", responseBundle) }
      },
      done: function () {
         if (doneCallback != null) { doneCallback(); }
      }
   });
};

function getUserTicketData(userTicket) {
   if (userTicket == null) { userTicket = { UserName: '', IsActiveUser: false, AccessExpirationSeconds: 1 }; }

   var userTicketData = {
      UserName: userTicket.UserName,
      IsActiveUser: userTicket.IsActiveUser,
      refreshSeconds: userTicket.AccessExpirationSeconds
   };
   userTicketData.IsTokenExpired = (userTicketData.refreshSeconds <= 1);
   userTicketData.refreshTime = moment().add(userTicketData.refreshSeconds, 'seconds').format('L LTS');

   return userTicketData;
};

function authIsTokenExpired() {
   if (authUserTicketData == null) { return true; }
   if (authUserTicketData.IsTokenExpired == null) { return true; }
   return authUserTicketData.IsTokenExpired;
};

function authIsActiveUser() {
   if (authUserTicketData == null) { return false; }
   if (authUserTicketData.IsActiveUser == null) { return false; }
   return authUserTicketData.IsActiveUser;
};
