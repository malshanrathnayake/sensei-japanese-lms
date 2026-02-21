CREATE PROCEDURE [dbo].[GetStudentRegistration]
	@studentRegistrationId BIGINT = 0
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
	WHERE STREG.StudentRegistrationId = @studentRegistrationId
	FOR JSON PATH

END
