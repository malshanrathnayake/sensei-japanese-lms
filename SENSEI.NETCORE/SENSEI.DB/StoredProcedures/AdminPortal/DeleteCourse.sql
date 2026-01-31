CREATE PROCEDURE [dbo].[DeleteCourse]
	@courseId BIGINT,
	@executionStatus BIT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY

		UPDATE Course SET IsDeleted = 1 WHERE CourseId = @courseId;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		SET @executionStatus = 0;
		RETURN;

	END CATCH;
	

END
