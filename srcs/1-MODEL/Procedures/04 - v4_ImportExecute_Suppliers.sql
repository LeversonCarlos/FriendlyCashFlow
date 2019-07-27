CREATE PROCEDURE dbo.v4_ImportExecute_Suppliers
   @iSupplier bigint output, 
   @Supplier_Code varchar(50), 
   @Supplier_Description varchar(500), 
   @iLogin bigint, @iUser nvarchar(max)
AS
BEGIN

   /* INITIALIZE */
   SET @iSupplier = 0;
   if (@Supplier_Code = '') begin return; end 

   /* SEARCH DATA BY CODE/DESCRIPTION */
   SELECT @iSupplier = idRow
   FROM v4_Suppliers
   WHERE 
      RowStatus = 1 And 
      CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser) And 
      (Code=@Supplier_Code Or [Description]=@Supplier_Description)
   if (@iSupplier <> 0) begin return; end

   /* INSERT NEW ROW */
   INSERT INTO v4_Suppliers(idSupplier, Code, [Description], RowStatus, CreatedBy, CreatedIn)
   VALUES(@iSupplier, @Supplier_Code, @Supplier_Description, 1, @iLogin, getdate())

   /* GET IDENTITY AND UPDATE ID COLUMN */
   SELECT @iSupplier = SCOPE_IDENTITY();
   UPDATE v4_Suppliers SET idSupplier=@iSupplier WHERE idRow=@iSupplier;

END
GO
