CREATE PROCEDURE [dbo].[SearchMyRegistrations]
	@email NVARCHAR(256),
	@start INT = 0,
	@length INT = 10,
	@count INT OUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT STREG.*,
		JSON_QUERY(ISNULL((SELECT Staff.* FROM Staff WHERE Staff.IsDeleted = 0 AND Staff.EmployeeId = STREG.ApprovedById FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'ApprovedBy',
		JSON_QUERY(ISNULL((SELECT Branch.* FROM Branch WHERE Branch.BranchId = STREG.BranchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Branch',
		JSON_QUERY(ISNULL((SELECT StudentLearningMode.* FROM StudentLearningMode WHERE StudentLearningMode.StudentLearningModeId = STREG.StudentLearningModeId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'StudentLearningMode',
		JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.CourseId = STREG.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
	FROM StudentRegistration STREG
	WHERE STREG.Email = @email
	ORDER BY STREG.CreatedDateTime DESC
	OFFSET @start ROWS
	FETCH NEXT @length ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(*)
	FROM StudentRegistration STREG
	WHERE STREG.Email = @email;

END
