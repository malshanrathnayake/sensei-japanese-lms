CREATE PROCEDURE [dbo].[DeleteBatchLesson]
	@batchLessonId BIGINT = 0,
	@executionStatus BIT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY

		UPDATE BatchLesson SET IsDeleted = 1 WHERE BatchLessonId = @batchLessonId
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		SET @executionStatus = 0;
		RETURN;

	END CATCH;

END
