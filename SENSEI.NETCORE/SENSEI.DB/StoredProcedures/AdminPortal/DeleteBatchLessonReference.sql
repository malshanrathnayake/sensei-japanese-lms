CREATE PROCEDURE [dbo].[DeleteBatchLessonReference]
	@batchLessonReferenceId BIGINT = 0,
	@executionStatus BIT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY

		DELETE FROM BatchLessonReference WHERE BatchLessonReferenceId = @batchLessonReferenceId
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		SET @executionStatus = 0;
		RETURN;

	END CATCH;

END
