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
		SELECT BL.BatchLessonId, @studentId, 1
		FROM BatchLesson BL
		LEFT JOIN BatchStudentLessonAccess BSLA ON BL.BatchLessonId = BSLA.BatchLessonId AND BSLA.StudentId = @studentId
		WHERE BL.BatchId = @batchId 
			AND BSLA.BatchStudentLessonAccessId IS NULL -- Only insert if access doesn't exist
			AND (
				(@lessonId IS NOT NULL AND BL.LessonId = @lessonId) -- Grant by Category
				OR 
				(@lessonId IS NULL AND MONTH(BL.LessonDateTime) = MONTH(@paymentMonth) AND YEAR(BL.LessonDateTime) = YEAR(@paymentMonth)) -- Grant by Calendar Month
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