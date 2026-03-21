CREATE PROCEDURE [dbo].[ApproveBatchStudentLessonRequest]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY

		DECLARE @BatchStudentLessonAccessRequestId BIGINT, @changeById BIGINT, @changedDate DATETIME, @batchLessonAccessId BIGINT;

		SELECT @BatchStudentLessonAccessRequestId = [BatchStudentLessonAccessRequestId], @changeById = [ChangeById], @changedDate = [ChangedDate]
		FROM OPENJSON(@jsonString, '$')
		WITH
		(
			[BatchStudentLessonAccessRequestId] BIGINT,
			[ApproveStatusEnum] BIGINT,
			[ChangeById] BIGINT,
			[ChangedDate] DATETIMEOFFSET
		);

		SELECT @batchLessonAccessId = [BatchStudentLessonAccessId] FROM BatchStudentLessonAccessRequest WHERE BatchStudentLessonAccessRequestId = @BatchStudentLessonAccessRequestId;

		UPDATE BatchStudentLessonAccessRequest SET ApproveStatusEnum = 1, ChangeById = @changeById, ChangeDate = GETUTCDATE() WHERE BatchStudentLessonAccessRequestId = @BatchStudentLessonAccessRequestId;

		UPDATE BatchStudentLessonAccess SET HasAccess = 1 WHERE BatchStudentLessonAccessId = @batchLessonAccessId;

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		THROW;

	END CATCH

END
