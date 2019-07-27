CREATE PROCEDURE dbo.v4_ImportExecute_History
   @iHistory bigint output, 
   @iDocument_Type smallint, 
   @History_DueDate varchar(10), 
   @History_Value varchar(20), 
   @History_Settled varchar(1), 
   @History_PayDate varchar(10), 
   @iDocument bigint, 
   @iAccount bigint, 
   @Transfer varchar(20), 
   @iLogin bigint, @iUser nvarchar(max)
AS
BEGIN

   /* INITIALIZE */
   SET @iHistory = 0;
   DECLARE @DueDate date; SET @DueDate = @History_DueDate;
   DECLARE @Value float; SET @Value = @History_Value;
   DECLARE @HistoryAccount bigint; SET @HistoryAccount=null; if(@iAccount<>0) begin SET @HistoryAccount=@iAccount; end
   DECLARE @PayDate date; SET @PayDate=null; if(@History_Settled='1') begin SET @PayDate=@History_PayDate; end
   DECLARE @iTransfer bigint; SET @iTransfer=null; if(@Transfer<>'0') begin SET @iTransfer=@Transfer; end

   /* INSERT NEW ROW */
   INSERT INTO v4_History(idHistory, idDocument, [Type], idTransfer, DueDate, Value, idAccount, [Settled], PayDate, Sorting, RowStatus, CreatedBy, CreatedIn)
   VALUES(@iHistory, @iDocument, @iDocument_Type, @iTransfer, @DueDate, @Value, @HistoryAccount, @History_Settled, @PayDate, 0, 1, @iLogin, getdate())

   /* GET IDENTITY AND UPDATE ID COLUMN */
   SELECT @iHistory = SCOPE_IDENTITY();

   /* SORTING */
   DECLARE @Initial date; SET @Initial='1901-01-01';
   DECLARE @Final date; if(@History_Settled='1') begin SET @Final=@PayDate; end else begin SET @Final=@DueDate; end
   DECLARE @Sorting float; SET @Sorting = datediff(d, @Initial, @Final);
   DECLARE @Length smallint; SET @Length = len(ltrim(str(@iHistory)));
   DECLARE @Power float; SET @Power = power(10, @Length)
   SET @Sorting = @Sorting + (@iHistory / @Power);

   /* UPDATE */
   UPDATE v4_History SET idHistory=@iHistory, Sorting=@Sorting WHERE idRow=@iHistory;

   /* DOCUMENT SETTLED: CHECK */
   DECLARE @SumQuantity int; DECLARE @SumSettled int; 
   select 
      @SumQuantity = count(*), 
      @SumSettled = sum((case when History.Settled=1 then 1 else 0 end)) 
   from v4_Documents As Documents
      join v4_History AS History On (History.idDocument=Documents.idRow And History.RowStatus=1)
   where Documents.idRow = @iDocument
   group by Documents.idRow

   /* DOCUMENT SETTLED: UPDATE */
   if (@SumSettled=@SumQuantity) begin
      UPDATE v4_Documents SET Settled=(case when @SumSettled=@SumQuantity then 1 else 0 end) WHERE idRow=@iDocument;
   end

END
GO
