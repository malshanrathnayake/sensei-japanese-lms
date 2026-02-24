CREATE PROCEDURE [dbo].[GetStudentPaymentSummary]
	@studentId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT SB.StudentBatchId, B.BatchName, C.CourseName, 
		(SELECT ISNULL(SUM(Amount), 0) FROM StudentBatchPayment WHERE StudentBatchId = SB.StudentBatchId AND IsDeleted = 0) AS TotalPaid,
		(SELECT ISNULL(SUM(Amount), 0) FROM StudentBatchPayment WHERE StudentBatchId = SB.StudentBatchId AND IsDeleted = 0 AND IsApproved = 1) AS TotalApproved
	FROM StudentBatch SB
	INNER JOIN Batch B ON B.BatchId = SB.BatchId
	INNER JOIN Course C ON C.CourseId = B.CourseId
	WHERE SB.StudentId = @studentId AND SB.IsDeleted = 0
	FOR JSON PATH;
END
