CREATE PROCEDURE [dbo].[UpdateStudentBatchPayment]
	@jsonString NVARCHAR(MAX),
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @paymentId BIGINT;
		SELECT @paymentId = StudentBatchPaymentId FROM OPENJSON(@jsonString) WITH (StudentBatchPaymentId BIGINT);

		IF @paymentId = 0
		BEGIN
			INSERT INTO StudentBatchPayment (StudentBatchId, PaymentMonth, Amount, PaymentDate, SlipUrl, IsApproved, LessonId)
			SELECT StudentBatchId, PaymentMonth, Amount, GETUTCDATE(), SlipUrl, 0, LessonId
			FROM OPENJSON(@jsonString)
			WITH (
				StudentBatchId BIGINT,
				PaymentMonth DATETIME,
				Amount DECIMAL(18, 2),
				SlipUrl NVARCHAR(MAX),
				LessonId BIGINT
			);
			SET @primaryKey = SCOPE_IDENTITY();
		END
		ELSE
		BEGIN
			UPDATE StudentBatchPayment
			SET StudentBatchId = J.StudentBatchId,
				PaymentMonth = J.PaymentMonth,
				Amount = J.Amount,
				SlipUrl = ISNULL(J.SlipUrl, SBP.SlipUrl),
				LessonId = ISNULL(J.LessonId, SBP.LessonId)
			FROM StudentBatchPayment SBP
			INNER JOIN OPENJSON(@jsonString)
			WITH (
				StudentBatchPaymentId BIGINT,
				StudentBatchId BIGINT,
				PaymentMonth DATETIME,
				Amount DECIMAL(18, 2),
				SlipUrl NVARCHAR(MAX),
				LessonId BIGINT
			) J ON SBP.StudentBatchPaymentId = J.StudentBatchPaymentId
			WHERE SBP.StudentBatchPaymentId = @paymentId;
			SET @primaryKey = @paymentId;
		END

		COMMIT TRANSACTION;
		SET @executionStatus = 1;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		THROW;
	END CATCH
END
