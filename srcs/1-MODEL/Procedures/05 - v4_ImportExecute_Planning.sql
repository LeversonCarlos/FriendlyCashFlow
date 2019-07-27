CREATE PROCEDURE dbo.v4_ImportExecute_Planning
   @iPlanning bigint output, 
   @Planning_Description varchar(1000), 
   @iDocument_Type smallint, 
   @iLogin bigint, @iUser nvarchar(max)
AS
BEGIN

   /* INITIALIZE */
   SET @iPlanning = 0;
   if (@Planning_Description = '') begin return; end 
   DECLARE @iParent bigint; SET @iParent=null;

   /* SPLIT */
   CREATE TABLE #Planning(Id bigint, Data varchar(4000));
   INSERT INTO #Planning SELECT * FROM dbo.v4_Split(@Planning_Description, '/');

   /* OPEN CURSOR */
   DECLARE @Planning varchar(4000); 
   DECLARE oCursor CURSOR FOR SELECT Data FROM #Planning;
   OPEN oCursor; FETCH NEXT FROM oCursor INTO @Planning;

   /* LOOP */
   WHILE @@FETCH_STATUS = 0 BEGIN
      if (@Planning <> '') begin
         SET @iPlanning = 0;

         /* SEARCH DATA */
         SELECT @iPlanning=idRow
         FROM v4_Planning
         WHERE 
            RowStatus = 1 And 
            CreatedBy in (SELECT idRow FROM v4_Logins WHERE idUser=@iUser) And 
            [Description]=@Planning And 
            coalesce(idParentRow,0) = coalesce(@iParent,0);

         if (@iPlanning = 0) begin 

            /* INSERT NEW ROW */
            INSERT INTO v4_Planning(idPlanning, [Description], [Type], idParentRow, RowStatus, CreatedBy, CreatedIn)
            VALUES(@iPlanning, @Planning, @iDocument_Type, @iParent, 1, @iLogin, getdate())

            /* GET IDENTITY AND UPDATE ID COLUMN */
            SELECT @iPlanning = SCOPE_IDENTITY();
            UPDATE v4_Planning SET idPlanning=@iPlanning WHERE idRow=@iPlanning;

         end

         /* STORE PARENT */
         SET @iParent = @iPlanning;

      end
      FETCH NEXT FROM oCursor INTO @Planning;
   END 

   /* CLOSE CURSOR */
   CLOSE oCursor;
   DEALLOCATE oCursor;

END
GO
