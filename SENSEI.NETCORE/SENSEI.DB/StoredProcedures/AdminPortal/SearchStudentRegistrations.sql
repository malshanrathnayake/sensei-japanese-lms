CREATE PROCEDURE [dbo].[SearchStudentRegistrations]
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

	SELECT STREG.*,
		JSON_QUERY(ISNULL((SELECT Staff.* FROM Staff WHERE Staff.IsDeleted = 0 AND Staff.EmployeeId = STREG.ApprovedById FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'ApprovedBy',
		JSON_QUERY(ISNULL((SELECT City.*,
			JSON_QUERY(ISNULL((SELECT [State].*,
				JSON_QUERY(ISNULL((SELECT Country.* FROM Country WHERE Country.CountryId = [State].CountryId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Country'
			FROM [State] WHERE [State].StateId = City.StateId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'State'
		FROM City WHERE City.CityId = STREG.CityId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'City',
		JSON_QUERY(ISNULL((SELECT Branch.* FROM Branch WHERE Branch.BranchId = STREG.BranchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Branch',
		JSON_QUERY(ISNULL((SELECT StudentLearningMode.* FROM StudentLearningMode WHERE StudentLearningMode.StudentLearningModeId = STREG.StudentLearningModeId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'StudentLearningMode',
		JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.CourseId = STREG.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
	FROM StudentRegistration STREG
	INNER JOIN Course C ON C.CourseId = STREG.CourseId
	INNER JOIN StudentLearningMode SLM ON SLM.StudentLearningModeId = STREG.StudentLearningModeId
	WHERE STREG.IsApproved = 0
		AND (@courseId = 0 OR STREG.CourseId = @courseId)
		AND (@searchValue = '' OR STREG.FirstName LIKE '%' + @searchValue + '%' OR STREG.LastName LIKE '%' + @searchValue + '%' OR STREG.MiddleName LIKE '%' + @searchValue + '%' OR CAST(STREG.CreatedDateTime AS DATE) LIKE '%' + @searchValue + '%' OR C.CourseName LIKE '%' + @searchValue + '%' OR SLM.LearningModeName LIKE '%' + @searchValue + '%' OR STREG.Email LIKE '%' + @searchValue + '%' OR STREG.PhoneNo LIKE '%' + @searchValue + '%')
	ORDER BY 
		CASE WHEN @sortColumn = 'course.courseName' AND @sortDirection = 'ASC' THEN C.CourseName END ASC,CASE WHEN @sortColumn = 'course.courseName' AND @sortDirection = 'DESC' THEN C.CourseName END DESC,
		CASE WHEN @sortColumn = 'studentLearningMode.learningModeName' AND @sortDirection = 'ASC' THEN SLM.LearningModeName END ASC,CASE WHEN @sortColumn = 'studentLearningMode.learningModeName' AND @sortDirection = 'DESC' THEN SLM.LearningModeName END DESC,
		CASE WHEN @sortColumn = 'studentRegistrationPopulatedName' AND @sortDirection = 'ASC' THEN STREG.FirstName END ASC,CASE WHEN @sortColumn = 'studentRegistrationPopulatedName' AND @sortDirection = 'DESC' THEN STREG.FirstName END DESC,
		CASE WHEN @sortColumn = 'email' AND @sortDirection = 'ASC' THEN STREG.Email END ASC,CASE WHEN @sortColumn = 'email' AND @sortDirection = 'DESC' THEN STREG.Email END DESC,
		CASE WHEN @sortColumn = 'phoneNo' AND @sortDirection = 'ASC' THEN STREG.PhoneNo END ASC,CASE WHEN @sortColumn = 'phoneNo' AND @sortDirection = 'DESC' THEN STREG.PhoneNo END DESC,
		CASE WHEN @sortColumn = 'createdDateTime' AND @sortDirection = 'ASC' THEN STREG.CreatedDateTime END ASC,CASE WHEN @sortColumn = 'createdDateTime' AND @sortDirection = 'DESC' THEN STREG.CreatedDateTime END DESC
	OFFSET @start ROWS
	FETCH NEXT @length ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(*)
	FROM StudentRegistration STREG
	INNER JOIN Course C ON C.CourseId = STREG.CourseId
	INNER JOIN StudentLearningMode SLM ON SLM.StudentLearningModeId = STREG.StudentLearningModeId
	WHERE STREG.IsApproved = 0
		AND (@courseId = 0 OR STREG.CourseId = @courseId)
		AND (@searchValue = '' OR STREG.FirstName LIKE '%' + @searchValue + '%' OR STREG.LastName LIKE '%' + @searchValue + '%' OR STREG.MiddleName LIKE '%' + @searchValue + '%' OR CAST(STREG.CreatedDateTime AS DATE) LIKE '%' + @searchValue + '%' OR C.CourseName LIKE '%' + @searchValue + '%' OR SLM.LearningModeName LIKE '%' + @searchValue + '%' OR STREG.Email LIKE '%' + @searchValue + '%' OR STREG.PhoneNo LIKE '%' + @searchValue + '%')
END
