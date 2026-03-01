CREATE PROCEDURE [dbo].[SearchStudentBatchPayments]
	@studentId BIGINT,
	@studentBatchId BIGINT = 0,
	@start INT = 0,
	@length INT = 10,
	@count INT OUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SBP.*,
		JSON_QUERY(ISNULL((SELECT StudentBatch.*,
			JSON_QUERY(ISNULL((SELECT Batch.*,
				JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.CourseId = Batch.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course' 
			FROM Batch WHERE Batch.BatchId = StudentBatch.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
			JSON_QUERY(ISNULL((SELECT Student.* FROM Student WHERE Student.StudentId = StudentBatch.StudentId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Student' 
		FROM StudentBatch WHERE StudentBatch.StudentBatchId = SBP.StudentBatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'StudentBatch' 
	FROM StudentBatchPayment SBP
	INNER JOIN StudentBatch SB ON SB.StudentBatchId = SBP.StudentBatchId
	WHERE SB.StudentId = @studentId 
		AND SBP.IsDeleted = 0
		AND (@studentBatchId = 0 OR SBP.StudentBatchId = @studentBatchId)
	ORDER BY SBP.PaymentDate DESC
	OFFSET @start ROWS
	FETCH NEXT @length ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(*)
	FROM StudentBatchPayment SBP
	INNER JOIN StudentBatch SB ON SB.StudentBatchId = SBP.StudentBatchId
	WHERE SB.StudentId = @studentId 
		AND SBP.IsDeleted = 0
		AND (@studentBatchId = 0 OR SBP.StudentBatchId = @studentBatchId);
END
