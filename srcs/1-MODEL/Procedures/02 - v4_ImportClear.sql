-- dbo.v4_ImportClear 8
CREATE PROCEDURE dbo.v4_ImportClear
   @iLogin bigint
AS
BEGIN

   /* PARAMETERS */
   DECLARE @iUser nvarchar(max);
   SELECT @iUser=idUser FROM v4_Logins WHERE idRow=@iLogin;

   /* CLEAR */
   DELETE FROM v4_History WHERE CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser);
   DELETE FROM v4_Recurrent WHERE CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser);
   DELETE FROM v4_Documents WHERE CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser);
   DELETE FROM v4_Accounts WHERE CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser);
   DELETE FROM v4_Suppliers WHERE CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser);
   DELETE FROM v4_Planning WHERE CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser);

END
GO
