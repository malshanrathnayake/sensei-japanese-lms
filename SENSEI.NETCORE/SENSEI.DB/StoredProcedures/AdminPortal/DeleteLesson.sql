CREATE PROCEDURE [dbo].[DeleteLesson]
	@lessonId BIGINT,
	@executionStatus BIT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY

		UPDATE Lesson SET IsDeleted = 1 WHERE LessonId = @lessonId;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		SET @executionStatus = 0;
		RETURN;

	END CATCH;
	

END
