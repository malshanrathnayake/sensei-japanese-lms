CREATE PROCEDURE [dbo].[GetStudentBatchPayment]
	@paymentId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT SBP.*,
		JSON_QUERY(ISNULL((SELECT SB.*,
			JSON_QUERY(ISNULL((SELECT Batch.*,
				JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.IsDeleted = 0 AND Course.CourseId = Batch.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
			FROM Batch WHERE Batch.IsDeleted = 0 AND Batch.BatchId = SB.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
			JSON_QUERY(ISNULL((SELECT Student.* FROM Student WHERE Student.IsDeleted = 0 AND Student.StudentId = SB.StudentId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Student'
		FROM StudentBatch SB WHERE SB.StudentBatchId = SBP.StudentBatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'StudentBatch'
	FROM StudentBatchPayment SBP
	WHERE SBP.StudentBatchPaymentId = @paymentId AND SBP.IsDeleted = 0
	FOR JSON PATH;
END
