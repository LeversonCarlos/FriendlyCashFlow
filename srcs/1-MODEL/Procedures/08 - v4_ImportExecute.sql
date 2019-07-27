-- dbo.v4_ImportExecute 1
CREATE PROCEDURE dbo.v4_ImportExecute
   @idRow bigint
AS
BEGIN

   /* STATUS */
   UPDATE v4_ImportData SET [Status]=-1 WHERE idRow=@idRow And [Status]=0;
   /*if (@@ROWCOUNT <> 1) begin return; end*/

   /* PARAMETERS */
   DECLARE @DataText varchar(4000); DECLARE @iLogin bigint; DECLARE @iUser nvarchar(max);
   SELECT @DataText=Data, @iLogin=CreatedBy FROM v4_ImportData WHERE idRow=@idRow;
   CREATE TABLE #Data(Id bigint, Data varchar(4000));
   INSERT INTO #Data SELECT * FROM dbo.v4_Split(@DataText, ';');
   SELECT @iUser=idUser FROM v4_Logins WHERE idRow=@iLogin;

   /* PARAMETERS */
   DECLARE @Document_Description varchar(500);  SELECT @Document_Description=Replace(Data,'"','') FROM #Data WHERE Id=1;
   DECLARE @Document_Type        varchar(50);   SELECT @Document_Type=       Replace(Data,'"','') FROM #Data WHERE Id=2;
   DECLARE @Supplier_Code        varchar(50);   SELECT @Supplier_Code=       Replace(Data,'"','') FROM #Data WHERE Id=3;
   DECLARE @Supplier_Description varchar(500);  SELECT @Supplier_Description=Replace(Data,'"','') FROM #Data WHERE Id=4;
   DECLARE @Planning_Description varchar(1000); SELECT @Planning_Description=Replace(Data,'"','') FROM #Data WHERE Id=5;
   DECLARE @History_DueDate      varchar(10);   SELECT @History_DueDate=     Replace(Data,'"','') FROM #Data WHERE Id=6;
   DECLARE @History_Value        varchar(20);   SELECT @History_Value=       Replace(Data,'"','') FROM #Data WHERE Id=7;
   DECLARE @History_Settled      varchar(1);    SELECT @History_Settled=     Replace(Data,'"','') FROM #Data WHERE Id=8;
   DECLARE @History_PayDate      varchar(10);   SELECT @History_PayDate=     Replace(Data,'"','') FROM #Data WHERE Id=9;
   DECLARE @Account_Code         varchar(50);   SELECT @Account_Code=        Replace(Data,'"','') FROM #Data WHERE Id=10;
   DECLARE @Account_Description  varchar(500);  SELECT @Account_Description= Replace(Data,'"','') FROM #Data WHERE Id=11;
   DECLARE @Transfer             varchar(20);   SELECT @Transfer=            Replace(Data,'"','') FROM #Data WHERE Id=12;

   /* DOCUMENT TYPE */
   DECLARE @iDocument_Type smallint;
   if (upper(@Document_Type) = upper('Expense')) begin SET @iDocument_Type=1; end
   if (upper(@Document_Type) = upper('Income')) begin SET @iDocument_Type=2; end

   /* ACCOUNT */
   DECLARE @iAccount bigint; 
   begin try
      exec dbo.v4_ImportExecute_Accounts @iAccount output, @Account_Code, @Account_Description, @iLogin, @iUser;
   end try
   begin catch
      UPDATE v4_ImportData SET [Status]=2,[Message]='Account['+ERROR_MESSAGE()+']' WHERE idRow=@idRow And [Status]=-1; return;
   end catch

   /* SUPPLIER */
   DECLARE @iSupplier bigint; 
   begin try
      exec dbo.v4_ImportExecute_Suppliers @iSupplier output, @Supplier_Code, @Supplier_Description, @iLogin, @iUser;
   end try
   begin catch
      UPDATE v4_ImportData SET [Status]=2,[Message]='Supplier['+ERROR_MESSAGE()+']' WHERE idRow=@idRow And [Status]=-1; return;
   end catch

   /* PLANNING */
   DECLARE @iPlanning bigint; 
   begin try
      exec dbo.v4_ImportExecute_Planning @iPlanning output, @Planning_Description, @iDocument_Type, @iLogin, @iUser;
   end try
   begin catch
      UPDATE v4_ImportData SET [Status]=2,[Message]='Planning['+ERROR_MESSAGE()+']' WHERE idRow=@idRow And [Status]=-1; return;
   end catch

   /* DOCUMENT */
   DECLARE @iDocument bigint; 
   begin try
      exec dbo.v4_ImportExecute_Documents @iDocument output, @Document_Description, @iDocument_Type, @iSupplier, @iPlanning, @Transfer, @iLogin, @iUser;
   end try
   begin catch
      UPDATE v4_ImportData SET [Status]=2,[Message]='Document['+ERROR_MESSAGE()+']' WHERE idRow=@idRow And [Status]=-1; return;
   end catch

   /* HISTORY */
   DECLARE @iHistory bigint; 
   begin try
      exec dbo.v4_ImportExecute_History @iHistory output, @iDocument_Type, @History_DueDate, @History_Value, @History_Settled, @History_PayDate, @iDocument, @iAccount, @Transfer, @iLogin, @iUser 
   end try
   begin catch
      UPDATE v4_ImportData SET [Status]=2,[Message]='History['+ERROR_MESSAGE()+']' WHERE idRow=@idRow And [Status]=-1; return;
   end catch

   /* STATUS */
   UPDATE v4_ImportData SET [Status]=1 WHERE idRow=@idRow And [Status]=-1;

   /* RETURN */
   DROP TABLE #Data
END
GO
