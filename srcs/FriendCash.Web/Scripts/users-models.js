function koUserModel(value) {
   var self = this;
   self.ID = ko.observable(value.ID);
   self.UserName = ko.observable(value.UserName);
   self.FullName = ko.observable(value.FullName);
   self.Email = ko.observable(value.Email);
   self.EmailConfirmed = ko.observable(value.EmailConfirmed);

   // DATE
   self.pureJoinDate = ko.observable(moment(value.JoinDate));
   self.JoinDate = ko.observable(self.pureJoinDate().format('L'));
   self.pureExpirationDate = ko.observable(moment(value.ExpirationDate));
   self.ExpirationDate = ko.observable(self.pureExpirationDate().format('L'));

   // ROLES
   self.UserRoles = ko.observable(new koUserRolesModel(value.UserRoles, self));

};

function koUserRolesModel(value, parent) {
   var self = this;
   self.Admin = ko.observable(value.Admin);
   self.User = ko.observable(value.User);
   self.Viewer = ko.observable(value.Viewer);
}