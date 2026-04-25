CREATE PROCEDURE [dbo].[SearchStudent]
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

	SELECT ST.*,
        U.IsActive AS IsActive,
		JSON_QUERY(ISNULL((SELECT StudentLearningMode.* FROM StudentLearningMode WHERE StudentLearningMode.StudentLearningModeId = ST.StudentLearningModeId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'StudentLearningMode',
		JSON_QUERY(ISNULL((SELECT StudentBatch.*,
			JSON_QUERY(ISNULL((SELECT Batch.*,
				JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.CourseId = Batch.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
			FROM Batch WHERE Batch.BatchId = StudentBatch.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch'
		FROM StudentBatch WHERE StudentBatch.StudentId = ST.StudentId FOR JSON PATH), '[]')) AS 'StudentBatches'
	FROM Student ST
    INNER JOIN [User] U ON U.UserId = ST.UserId
	INNER JOIN StudentBatch STB ON STB.StudentId = ST.StudentId
	INNER JOIN Batch B ON B.BatchId = STB.BatchId
	INNER JOIN Course C ON C.CourseId = B.CourseId
	INNER JOIN StudentLearningMode SLM ON SLM.StudentLearningModeId = ST.StudentLearningModeId
	WHERE 
		ST.IsDeleted = 0
		AND STB.IsDeleted = 0
		AND (@courseId = 0 OR B.CourseId = @courseId)
		AND (@batchId = 0 OR B.BatchId = @batchId)
		AND (@searchValue = '' OR ST.FirstName LIKE '%' + @searchValue + '%' OR ST.LastName LIKE '%' + @searchValue + '%' OR ST.MiddleName LIKE '%' + @searchValue + '%' OR ST.IndexNumber LIKE '%' + @searchValue + '%' OR C.CourseName LIKE '%' + @searchValue + '%' OR SLM.LearningModeName LIKE '%' + @searchValue + '%' OR ST.Email LIKE '%' + @searchValue + '%' OR ST.PhoneNo LIKE '%' + @searchValue + '%')
	ORDER BY 
		CASE WHEN @sortColumn = 'studentPopulatedName' AND @sortDirection = 'ASC' THEN ST.FirstName END ASC,CASE WHEN @sortColumn = 'studentPopulatedName' AND @sortDirection = 'DESC' THEN ST.FirstName END DESC,
		CASE WHEN @sortColumn = 'courseName' AND @sortDirection = 'ASC' THEN C.CourseName END ASC,CASE WHEN @sortColumn = 'courseName' AND @sortDirection = 'DESC' THEN C.CourseName END DESC,
		CASE WHEN @sortColumn = 'batchName' AND @sortDirection = 'ASC' THEN B.BatchName END ASC,CASE WHEN @sortColumn = 'batchName' AND @sortDirection = 'DESC' THEN B.BatchName END DESC,
		CASE WHEN @sortColumn = 'indexNumber' AND @sortDirection = 'ASC' THEN ST.IndexNumber END ASC,CASE WHEN @sortColumn = 'indexNumber' AND @sortDirection = 'DESC' THEN ST.IndexNumber END DESC,
		CASE WHEN @sortColumn = 'studentLearningMode.learningModeName' AND @sortDirection = 'ASC' THEN SLM.LearningModeName END ASC,CASE WHEN @sortColumn = 'studentLearningMode.learningModeName' AND @sortDirection = 'DESC' THEN SLM.LearningModeName END DESC,
		CASE WHEN @sortColumn = 'email' AND @sortDirection = 'ASC' THEN ST.Email END ASC,CASE WHEN @sortColumn = 'email' AND @sortDirection = 'DESC' THEN ST.Email END DESC,
		CASE WHEN @sortColumn = 'phoneNo' AND @sortDirection = 'ASC' THEN ST.PhoneNo END ASC,CASE WHEN @sortColumn = 'phoneNo' AND @sortDirection = 'DESC' THEN ST.PhoneNo END DESC,
		CASE WHEN @sortColumn = 'studentId' AND @sortDirection = 'ASC' THEN ST.StudentId END ASC,CASE WHEN @sortColumn = 'studentId' AND @sortDirection = 'DESC' THEN ST.StudentId END DESC
	OFFSET @start ROWS
	FETCH NEXT (CASE WHEN @length = -1 THEN 2147483647 ELSE @length END) ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(DISTINCT ST.StudentId)
	FROM Student ST
    INNER JOIN [User] U ON U.UserId = ST.UserId
	INNER JOIN StudentBatch STB ON STB.StudentId = ST.StudentId
	INNER JOIN Batch B ON B.BatchId = STB.BatchId
	INNER JOIN Course C ON C.CourseId = B.CourseId
	INNER JOIN StudentLearningMode SLM ON SLM.StudentLearningModeId = ST.StudentLearningModeId
	WHERE 
		ST.IsDeleted = 0
		AND STB.IsDeleted = 0
		AND (@courseId = 0 OR B.CourseId = @courseId)
		AND (@searchValue = '' OR ST.FirstName LIKE '%' + @searchValue + '%' OR ST.LastName LIKE '%' + @searchValue + '%' OR ST.MiddleName LIKE '%' + @searchValue + '%' OR ST.IndexNumber LIKE '%' + @searchValue + '%' OR C.CourseName LIKE '%' + @searchValue + '%' OR SLM.LearningModeName LIKE '%' + @searchValue + '%' OR ST.Email LIKE '%' + @searchValue + '%' OR ST.PhoneNo LIKE '%' + @searchValue + '%')

END
