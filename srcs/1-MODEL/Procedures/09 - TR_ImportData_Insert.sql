CREATE TRIGGER dbo.TR_ImportData_Insert
ON dbo.v4_ImportData
AFTER INSERT     
AS       
BEGIN   

   DECLARE @idRow bigint;       
   SELECT @idRow=idRow FROM inserted;

   EXEC dbo.v4_ImportExecute @idRow

END
GO
