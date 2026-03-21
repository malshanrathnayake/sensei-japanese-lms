CREATE PROCEDURE [dbo].[UpdateBatchLessonAccess]
	@jsonString NVARCHAR(MAX),
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	BEGIN TRY

		INSERT INTO [BatchStudentLessonAccessRequest]([BatchStudentLessonAccessId], [RequestedDate], [RequestEndDate])
		SELECT [BatchStudentLessonAccessId], [RequestedDate], [RequestEndDate]
		FROM OPENJSON(@jsonString)
		WITH (
			BatchStudentLessonAccessId BIGINT,
			RequestedDate DATETIMEOFFSET,
			RequestEndDate DATETIMEOFFSET
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