CREATE PROCEDURE [dbo].[SearchStudentPayments]
	@courseId BIGINT = 0,
	@batchId BIGINT = 0,
	@start INT = 0,
	@length INT = 10,
	@searchValue NVARCHAR(MAX),
	@sortColumn NVARCHAR(MAX),
	@sortDirection NVARCHAR(MAX),
	@count INT OUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT SBP.*,
		JSON_QUERY(ISNULL((SELECT StudentBatch.*,
			JSON_QUERY(ISNULL((SELECT Batch.*,
				JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.IsDeleted = 0 AND Course.CourseId = Batch.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
			FROM Batch WHERE Batch.IsDeleted = 0 AND Batch.BatchId = StudentBatch.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
			JSON_QUERY(ISNULL((SELECT Student.* FROM Student WHERE Student.IsDeleted = 0 AND Student.StudentId = StudentBatch.StudentId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Student'
		FROM StudentBatch WHERE StudentBatch.IsDeleted = 0 AND StudentBatch.StudentBatchId = SBP.StudentBatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'StudentBatch',
		JSON_QUERY(ISNULL((SELECT Staff.* FROM Staff WHERE Staff.IsDeleted = 0 AND Staff.EmployeeId = SBP.ApprovedById FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'ApprovedBy'
	FROM StudentBatchPayment SBP
	INNER JOIN StudentBatch ON StudentBatch.StudentBatchId = SBP.StudentBatchId
	INNER JOIN Batch ON Batch.BatchId = StudentBatch.BatchId
	INNER JOIN Course ON Course.CourseId = Batch.CourseId
	LEFT JOIN Staff ON Staff.EmployeeId = SBP.ApprovedById
	WHERE SBP.IsDeleted = 0
		AND (@courseId = 0 OR Batch.CourseId = @courseId)
		AND (@batchId = 0 OR StudentBatch.BatchId = @batchId)
		AND (@searchValue = '' OR Course.CourseName LIKE '%' + @searchValue + '%' OR Batch.BatchName LIKE '%' + @searchValue + '%' OR CAST(SBP.PaymentMonth AS DATE) LIKE '%' + @searchValue + '%' OR SBP.Amount LIKE '%' + @searchValue + '%' OR Staff.FirstName LIKE '%' + @searchValue + '%' OR Staff.LastName LIKE '%' + @searchValue + '%')
	ORDER BY 
		CASE WHEN @sortColumn = 'studentBatch.batch.course.courseName' AND @sortDirection = 'ASC' THEN Course.CourseName END ASC,CASE WHEN @sortColumn = 'studentBatch.batch.course.courseName' AND @sortDirection = 'DESC' THEN Course.CourseName END DESC,
		CASE WHEN @sortColumn = 'studentBatch.batch.batchName' AND @sortDirection = 'ASC' THEN Batch.BatchName END ASC,CASE WHEN @sortColumn = 'studentBatch.batch.batchName' AND @sortDirection = 'DESC' THEN Batch.BatchName END DESC,
		CASE WHEN @sortColumn = 'paymentMonth' AND @sortDirection = 'ASC' THEN SBP.PaymentMonth END ASC,CASE WHEN @sortColumn = 'paymentMonth' AND @sortDirection = 'DESC' THEN SBP.PaymentMonth END DESC,
		CASE WHEN @sortColumn = 'amount' AND @sortDirection = 'ASC' THEN SBP.Amount END ASC,CASE WHEN @sortColumn = 'amount' AND @sortDirection = 'DESC' THEN SBP.Amount END DESC,
		CASE WHEN @sortColumn = 'approvedBy.staffPopulatedName' AND @sortDirection = 'ASC' THEN Staff.FirstName + ' ' + Staff.LastName END ASC,CASE WHEN @sortColumn = 'approvedBy.staffPopulatedName' AND @sortDirection = 'DESC' THEN Staff.FirstName + ' ' + Staff.LastName END DESC,
		CASE WHEN @sortColumn = 'changeDateTIme' AND @sortDirection = 'ASC' THEN SBP.ChangeDateTIme END ASC,CASE WHEN @sortColumn = 'changeDateTIme' AND @sortDirection = 'DESC' THEN SBP.ChangeDateTIme END DESC
	OFFSET @start ROWS
	FETCH NEXT (CASE WHEN @length = -1 THEN 2147483647 ELSE @length END) ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(*) 
	FROM StudentBatchPayment SBP
	INNER JOIN StudentBatch ON StudentBatch.StudentBatchId = SBP.StudentBatchId
	INNER JOIN Batch ON Batch.BatchId = StudentBatch.BatchId
	INNER JOIN Course ON Course.CourseId = Batch.CourseId
	LEFT JOIN Staff ON Staff.EmployeeId = SBP.ApprovedById
	WHERE SBP.IsDeleted = 0
		AND (@courseId = 0 OR Batch.CourseId = @courseId)
		AND (@batchId = 0 OR StudentBatch.BatchId = @batchId)
		AND (@searchValue = '' OR Course.CourseName LIKE '%' + @searchValue + '%' OR Batch.BatchName LIKE '%' + @searchValue + '%' OR CAST(SBP.PaymentMonth AS DATE) LIKE '%' + @searchValue + '%' OR SBP.Amount LIKE '%' + @searchValue + '%' OR Staff.FirstName LIKE '%' + @searchValue + '%' OR Staff.LastName LIKE '%' + @searchValue + '%')

END