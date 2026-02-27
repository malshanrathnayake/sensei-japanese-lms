CREATE PROCEDURE [dbo].[GetStudentBatchPayment]
	@paymentId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT SBP.*,
		JSON_QUERY(ISNULL((SELECT SB.* FROM StudentBatch SB WHERE SB.StudentBatchId = SBP.StudentBatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'StudentBatch'
	FROM StudentBatchPayment SBP
	WHERE SBP.StudentBatchPaymentId = @paymentId AND SBP.IsDeleted = 0
	FOR JSON PATH;
END
