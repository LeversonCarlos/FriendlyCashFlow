CREATE PROCEDURE dbo.v4_ImportExecute_Accounts
   @iAccount bigint output, 
   @Account_Code varchar(50), 
   @Account_Description varchar(500), 
   @iLogin bigint, @iUser nvarchar(max)
AS
BEGIN

   /* INITIALIZE */
   SET @iAccount = 0;
   if (@Account_Code = '') begin return; end 

   /* SEARCH DATA BY CODE/DESCRIPTION */
   SELECT @iAccount = idRow
   FROM v4_Accounts
   WHERE 
      RowStatus = 1 And 
      CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser) And 
      (Code=@Account_Code Or [Description]=@Account_Description)
   if (@iAccount <> 0) begin return; end

   /* INSERT NEW ROW */
   INSERT INTO v4_Accounts(idAccount, Code, [Description], [Type], [Status], RowStatus, CreatedBy, CreatedIn)
   VALUES(@iAccount, @Account_Code, @Account_Description, 0, 1, 1, @iLogin, getdate())

   /* GET IDENTITY AND UPDATE ID COLUMN */
   SELECT @iAccount = SCOPE_IDENTITY();
   UPDATE v4_Accounts SET idAccount=@iAccount WHERE idRow=@iAccount;

END
GO
