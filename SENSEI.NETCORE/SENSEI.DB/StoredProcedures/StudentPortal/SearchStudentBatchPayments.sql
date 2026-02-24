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
		JSON_QUERY(ISNULL((
			SELECT B.*, 
				JSON_QUERY(ISNULL((SELECT C.* FROM Course C WHERE C.CourseId = B.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course' 
			FROM Batch B 
			INNER JOIN StudentBatch SB ON SB.BatchId = B.BatchId 
			WHERE SB.StudentBatchId = SBP.StudentBatchId 
			FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
		), null)) AS 'BatchDetails'
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
