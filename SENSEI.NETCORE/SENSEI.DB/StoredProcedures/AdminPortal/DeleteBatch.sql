CREATE PROCEDURE [dbo].[DeleteBatch]
	@batchId BIGINT,
	@executionStatus BIT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY

		UPDATE Batch SET IsDeleted = 1 WHERE BatchId = @batchId;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		SET @executionStatus = 0;
		RETURN;

	END CATCH;
	

END
