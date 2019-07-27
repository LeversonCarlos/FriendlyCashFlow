-- EXEC dbo.[v4_PlanningChildren] 'ce7f2c98-fb34-47a9-976c-e897df497b1e', 2, 0
CREATE PROCEDURE [dbo].[v4_PlanningChildren]
   @idUser varchar(max), 
   @Type smallint, 
   @idPlanning bigint
AS
BEGIN 

   CREATE TABLE #Planning(idPlanning bigint);

   INSERT INTO #Planning
   SELECT Planning.idPlanning
   FROM v4_Planning As Planning
      INNER JOIN v4_Logins As Logins On (Logins.idRow = Planning.CreatedBy)
   WHERE 
      Logins.idUser = @idUser And 
      Planning.RowStatus = 1 And 
      Planning.Type = @Type And 
      ((@idPlanning = 0 And coalesce(Planning.idParentRow,0)=0) Or Planning.idPlanning = @idPlanning)

   DECLARE @countCurrent int; SELECT @countCurrent = count(*) FROM #Planning;
   DECLARE @countAfter int; SET @countAfter = 0;

   WHILE @countCurrent <> @countAfter BEGIN
      SET @countAfter = @countCurrent;

      INSERT INTO #Planning 
      SELECT Planning.idPlanning 
      FROM v4_Planning As Planning 
      WHERE 
         Planning.idParentRow in (SELECT idPlanning FROM #Planning) And 
         not Planning.idPlanning in (SELECT idPlanning FROM #Planning) 

      SELECT @countCurrent = count(*) FROM #Planning;
   END

   /* RETURN */
   --SELECT * FROM v4_Planning WHERE idPlanning in (SELECT idPlanning FROM #Planning);
   SELECT idPlanning FROM #Planning
   DROP TABLE #Planning;
END
GO
