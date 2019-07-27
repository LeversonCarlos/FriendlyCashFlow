CREATE PROCEDURE dbo.v4_ImportExecute_Documents
   @iDocument bigint output, 
   @Document_Description varchar(500), 
   @iDocument_Type smallint, 
   @iSupplier bigint, 
   @iPlanning bigint, 
   @Transfer varchar(20), 
   @iLogin bigint, @iUser nvarchar(max)
AS
BEGIN

   /* INITIALIZE */
   SET @iDocument = 0;
   if (@Document_Description = '' And @Transfer <> '') begin SET @Document_Description='Transfer'; end 
   if (@Document_Description = '') begin return; end 

   /* SEARCH DATA */
   SELECT @iDocument = idRow
   FROM v4_Documents
   WHERE 
      RowStatus = 1 And 
      CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser) And 
      [Description]=@Document_Description And 
      [Type]=@iDocument_Type
   if (@iDocument <> 0) begin return; end

   /* INSERT NEW ROW */
   DECLARE @iType smallint; SET @iType = @iDocument_Type; if(@Transfer <> '0') begin SET @iType=3; end
   DECLARE @DocumentSupplier bigint; SET @DocumentSupplier=null; if(@iSupplier<>0) begin SET @DocumentSupplier=@iSupplier; end
   DECLARE @DocumentPlanning bigint; SET @DocumentPlanning=null; if(@iPlanning<>0) begin SET @DocumentPlanning=@iPlanning; end
   INSERT INTO v4_Documents(idDocument, [Description], [Type], [Settled], idSupplier, idPlanning, RowStatus, CreatedBy, CreatedIn)
   VALUES(@iDocument, @Document_Description, @iType, 0, @DocumentSupplier, @DocumentPlanning, 1, @iLogin, getdate())

   /* GET IDENTITY AND UPDATE ID COLUMN */
   SELECT @iDocument = SCOPE_IDENTITY();
   UPDATE v4_Documents SET idDocument=@iDocument WHERE idRow=@iDocument;

END
GO
