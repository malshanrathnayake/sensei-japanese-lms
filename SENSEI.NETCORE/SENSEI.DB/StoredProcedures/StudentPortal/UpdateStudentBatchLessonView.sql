CREATE PROCEDURE [dbo].[UpdateStudentBatchLessonView]
	@jsonString NVARCHAR(MAX),
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	BEGIN TRY

		INSERT INTO StudentBatchLessonView([StudentId], [BatchLessonId], [LessonId], [IsCompleted])
		SELECT [StudentId], [BatchLessonId], [LessonId], [IsCompleted]
		FROM OPENJSON(@jsonString)
		WITH (
			StudentId BIGINT,
			BatchLessonId BIGINT,
			LessonId BIGINT,
			IsCompleted BIT
		);

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		THROW;

	END CATCH
END