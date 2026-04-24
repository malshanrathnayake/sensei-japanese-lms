CREATE PROCEDURE [dbo].[ApproveStudentBatchPayment]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY

		DECLARE @studentBatchPaymentId BIGINT, @approvedById BIGINT, @batchId BIGINT, @studentId BIGINT, @paymentMonth DATETIME, @lessonId BIGINT

		SELECT @studentBatchPaymentId = [StudentBatchPaymentId], @approvedById = [ApprovedById], @paymentMonth = [PaymentMonth], @lessonId = [LessonId]
		FROM OPENJSON(@jsonString, '$')
		WITH
		(
			[StudentBatchPaymentId] BIGINT,
			[ApprovedById] BIGINT,
			[PaymentMonth] DATETIMEOFFSET,
			[LessonId] BIGINT
		);

		SELECT @batchId = BatchId, @studentId = StudentId, @lessonId = ISNULL(@lessonId, LessonId)
		FROM StudentBatchPayment
		JOIN StudentBatch ON StudentBatch.StudentBatchId = StudentBatchPayment.StudentBatchId
		WHERE StudentBatchPaymentId = @studentBatchPaymentId;

		UPDATE [StudentBatchPayment] SET [IsApproved] = 1, [ApprovedById] = @approvedById, [ChangeDateTIme] = GETUTCDATE()
		WHERE [StudentBatchPaymentId] = @studentBatchPaymentId;

		INSERT INTO BatchStudentLessonAccess([BatchLessonId], [StudentId], [HasAccess])
		SELECT BatchLessonId , @studentId, 1
		FROM BatchLesson BL
		WHERE BL.BatchId = @batchId 
			AND (@lessonId IS NULL OR BL.LessonId = @lessonId)
			AND (@lessonId IS NOT NULL OR (MONTH(BL.LessonDateTime) = MONTH(@paymentMonth) AND YEAR(BL.LessonDateTime) = YEAR(@paymentMonth)))

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		THROW;

	END CATCH

END