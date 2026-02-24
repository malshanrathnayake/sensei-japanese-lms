CREATE PROCEDURE [dbo].[SearchStudentCourses]
	@studentId BIGINT,
	@start INT = 0,
	@length INT = 10,
	@searchValue NVARCHAR(MAX) = '',
	@sortColumn NVARCHAR(MAX) = 'courseName',
	@sortDirection NVARCHAR(MAX) = 'ASC',
	@count INT OUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT C.*
	FROM Course C
	INNER JOIN Batch B ON B.CourseId = C.CourseId
	INNER JOIN StudentBatch SB ON SB.BatchId = B.BatchId
	WHERE C.IsDeleted = 0 
		AND SB.StudentId = @studentId 
		AND SB.IsDeleted = 0
		AND (@searchValue = '' OR C.CourseName LIKE '%' + @searchValue + '%')
	ORDER BY 
		CASE WHEN @sortColumn = 'courseName' AND @sortDirection = 'ASC' THEN C.CourseName END ASC,CASE WHEN @sortColumn = 'courseName' AND @sortDirection = 'DESC' THEN C.CourseName END DESC
	OFFSET @start ROWS
	FETCH NEXT @length ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(DISTINCT C.CourseId)
	FROM Course C
	INNER JOIN Batch B ON B.CourseId = C.CourseId
	INNER JOIN StudentBatch SB ON SB.BatchId = B.BatchId
	WHERE C.IsDeleted = 0 
		AND SB.StudentId = @studentId 
		AND SB.IsDeleted = 0
		AND (@searchValue = '' OR C.CourseName LIKE '%' + @searchValue + '%');

END
