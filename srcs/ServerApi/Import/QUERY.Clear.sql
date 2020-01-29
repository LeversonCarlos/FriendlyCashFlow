declare @resourceID varchar(128) = @paramResourceID;

delete from v6_dataEntries where ResourceID=@resourceID;
delete from v6_dataBalance where ResourceID=@resourceID;
delete from v6_dataRecurrencies where ResourceID=@resourceID;
delete from v6_dataPatterns where ResourceID=@resourceID;
delete from v6_dataCategories where ResourceID=@resourceID;
delete from v6_dataAccounts where ResourceID=@resourceID;

select cast(1 as bit) as OK;
