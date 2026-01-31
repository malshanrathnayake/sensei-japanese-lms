CREATE PROCEDURE [dbo].[GetCourses]
	@start INT = 0,
	@length INT = 10,
	@searchValue NVARCHAR(MAX),
	@sortColumn NVARCHAR(MAX),
	@sortDirection NVARCHAR(MAX),
	@count INT OUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT * 
	FROM Course 
	WHERE IsDeleted = 0
		AND (@searchValue = '' OR CourseName LIKE '%' + @searchValue + '%' OR CourseCode LIKE '%' + @searchValue + '%')
	ORDER BY 
	CASE WHEN @sortColumn = 'courseName' AND @sortDirection = 'ASC' THEN CourseName END ASC,CASE WHEN @sortColumn = 'courseName' AND @sortDirection = 'DESC' THEN CourseName END DESC,
	CASE WHEN @sortColumn = 'courseCode' AND @sortDirection = 'ASC' THEN CourseCode END ASC,CASE WHEN @sortColumn = 'courseCode' AND @sortDirection = 'DESC' THEN CourseCode END DESC
	OFFSET @start ROWS
	FETCH NEXT @length ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(*)
	FROM Course 
	WHERE IsDeleted = 0
		AND (@searchValue = '' OR CourseName LIKE '%' + @searchValue + '%' OR CourseCode LIKE '%' + @searchValue + '%')

END
