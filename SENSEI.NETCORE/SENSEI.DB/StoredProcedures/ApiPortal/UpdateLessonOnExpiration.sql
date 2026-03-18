CREATE PROCEDURE [dbo].[UpdateLessonOnExpiration]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY

		UPDATE [BatchStudentLessonAccess]
		SET
			HasAccess = 0

		FROM [BatchStudentLessonAccess] BSLA
		INNER JOIN [BatchLesson] BL ON BSLA.BatchLessonId = BL.BatchLessonId
		WHERE BL.RecordingExpireDate < GETUTCDATE();


		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
        THROW;

    END CATCH

END
