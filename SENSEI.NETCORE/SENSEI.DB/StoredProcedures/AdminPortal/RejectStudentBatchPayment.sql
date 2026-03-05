CREATE PROCEDURE [dbo].[RejectStudentBatchPayment]
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

		UPDATE [StudentBatchPayment] SET [IsApproved] = 0, [ApprovedById] = @approvedById, [ChangeDateTIme] = GETUTCDATE(), [IsRejected] = 1
		WHERE [StudentBatchPaymentId] = @studentBatchPaymentId;

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		THROW;

	END CATCH

END