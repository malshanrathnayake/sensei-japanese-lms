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
			INSERT INTO StudentBatchPayment (StudentBatchId, PaymentMonth, Amount, PaymentDate, SlipUrl, IsApproved, LessonId, ReferenceNumber, Remarks)
			SELECT StudentBatchId, PaymentMonth, Amount, GETUTCDATE(), SlipUrl, 0, LessonId, ReferenceNumber, Remarks
			FROM OPENJSON(@jsonString)
			WITH (
				StudentBatchId BIGINT,
				PaymentMonth DATETIME,
				Amount DECIMAL(18, 2),
				SlipUrl NVARCHAR(MAX),
				LessonId BIGINT,
				ReferenceNumber NVARCHAR(100),
				Remarks NVARCHAR(500)
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
				LessonId = ISNULL(J.LessonId, SBP.LessonId),
				ReferenceNumber = J.ReferenceNumber,
				Remarks = J.Remarks
			FROM StudentBatchPayment SBP
			INNER JOIN OPENJSON(@jsonString)
			WITH (
				StudentBatchPaymentId BIGINT,
				StudentBatchId BIGINT,
				PaymentMonth DATETIME,
				Amount DECIMAL(18, 2),
				SlipUrl NVARCHAR(MAX),
				LessonId BIGINT,
				ReferenceNumber NVARCHAR(100),
				Remarks NVARCHAR(500)
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
