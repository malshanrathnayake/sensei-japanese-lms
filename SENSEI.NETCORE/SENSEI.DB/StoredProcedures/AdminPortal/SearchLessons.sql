CREATE PROCEDURE [dbo].[SearchLessons]
	@courseId BIGINT = 0,
	@start INT = 0,
	@length INT = 10,
	@searchValue NVARCHAR(MAX),
	@sortColumn NVARCHAR(MAX),
	@sortDirection NVARCHAR(MAX),
	@count INT OUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT LSN.*,
		JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.IsDeleted = 0 AND Course.CourseId = LSN.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
	FROM Lesson LSN
	WHERE LSN.IsDeleted = 0
		AND (@courseId = 0 OR LSN.CourseId = @courseId)
		AND (@searchValue = '' OR LSN.LessonName LIKE '%' + @searchValue + '%' OR LSN.[Description] LIKE '%' + @searchValue + '%') 
	ORDER BY 
		CASE WHEN @sortColumn = 'lessonName' AND @sortDirection = 'ASC' THEN LSN.LessonName END ASC,CASE WHEN @sortColumn = 'lessonName' AND @sortDirection = 'DESC' THEN LSN.LessonName END DESC,
		CASE WHEN @sortColumn = 'lessonCode' AND @sortDirection = 'ASC' THEN LSN.[Description] END ASC,CASE WHEN @sortColumn = 'lessonCode' AND @sortDirection = 'DESC' THEN LSN.[Description] END DESC
	OFFSET @start ROWS
	FETCH NEXT @length ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(*)
	FROM Lesson LSN
	WHERE LSN.IsDeleted = 0
		AND (@courseId = 0 OR LSN.CourseId = @courseId)
		AND (@searchValue = '' OR LSN.LessonName LIKE '%' + @searchValue + '%' OR LSN.[Description] LIKE '%' + @searchValue + '%') 

END