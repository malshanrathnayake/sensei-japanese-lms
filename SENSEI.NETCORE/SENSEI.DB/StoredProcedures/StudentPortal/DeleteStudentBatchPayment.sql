CREATE PROCEDURE [dbo].[DeleteStudentBatchPayment]
	@paymentId BIGINT,
	@status BIT OUT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE StudentBatchPayment SET IsDeleted = 1 WHERE StudentBatchPaymentId = @paymentId;
	SET @status = 1;
END
