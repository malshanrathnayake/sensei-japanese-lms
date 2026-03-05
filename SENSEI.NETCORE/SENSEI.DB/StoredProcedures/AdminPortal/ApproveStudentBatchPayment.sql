CREATE PROCEDURE [dbo].[ApproveStudentBatchPayment]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY

		DECLARE @studentBatchPaymentId BIGINT, @approvedById BIGINT, @batchId BIGINT, @studentId BIGINT;

		SELECT @studentBatchPaymentId = [StudentBatchPaymentId], @approvedById = [ApprovedById]
		FROM OPENJSON(@jsonString, '$')
		WITH
		(
			[StudentBatchPaymentId] BIGINT,
			[ApprovedById] BIGINT
		);

		SELECT @batchId = BatchId, @studentId = StudentId
		FROM StudentBatch
		WHERE StudentBatchId = (SELECT StudentBatchId FROM StudentBatchPayment WHERE StudentBatchPaymentId = @studentBatchPaymentId)

		UPDATE [StudentBatchPayment] SET [IsApproved] = 1, [ApprovedById] = @approvedById, [ChangeDateTIme] = GETUTCDATE()
		WHERE [StudentBatchPaymentId] = @studentBatchPaymentId;

		INSERT INTO BatchStudentLessonAccess([BatchLessonId], [StudentId], [HasAccess])
		SELECT BatchLessonId , @studentId, 1
		FROM BatchLesson BL
		WHERE BL.BatchId = @batchId

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		THROW;

	END CATCH

END