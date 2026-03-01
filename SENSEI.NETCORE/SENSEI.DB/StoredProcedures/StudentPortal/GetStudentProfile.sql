CREATE PROCEDURE [dbo].[GetStudentProfile]
	@studentId BIGINT = 0,
	@userId BIGINT = 0
AS
BEGIN

	SET NOCOUNT ON;

	IF @studentId <> 0
	BEGIN
		SELECT S.*,
			JSON_QUERY(ISNULL((SELECT [User].* FROM [User] WHERE [User].UserId = S.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'User',
			JSON_QUERY(ISNULL((SELECT Branch.* FROM Branch WHERE Branch.BranchId = S.BranchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Branch',
			JSON_QUERY(ISNULL((SELECT StudentLearningMode.* FROM StudentLearningMode WHERE StudentLearningMode.StudentLearningModeId = S.StudentLearningModeId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'StudentLearningMode',
			JSON_QUERY(ISNULL((SELECT Country.* FROM Country WHERE Country.CountryId = S.CountryId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Country',
			JSON_QUERY(ISNULL((SELECT StudentBatch.* FROM StudentBatch WHERE StudentBatch.StudentId = S.StudentId FOR JSON PATH), '[]')) AS 'StudentBatches'
		FROM Student S
		WHERE S.StudentId = @studentId AND S.IsDeleted = 0
		FOR JSON PATH;
	END
	ELSE IF @userId <> 0
	BEGIN
		SELECT S.*,
			JSON_QUERY(ISNULL((SELECT [User].* FROM [User] WHERE [User].UserId = S.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'User',
			JSON_QUERY(ISNULL((SELECT Branch.* FROM Branch WHERE Branch.BranchId = S.BranchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Branch',
			JSON_QUERY(ISNULL((SELECT StudentLearningMode.* FROM StudentLearningMode WHERE StudentLearningMode.StudentLearningModeId = S.StudentLearningModeId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'StudentLearningMode',
			JSON_QUERY(ISNULL((SELECT Country.* FROM Country WHERE Country.CountryId = S.CountryId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Country',
			JSON_QUERY(ISNULL((SELECT StudentBatch.* FROM StudentBatch WHERE StudentBatch.StudentId = S.StudentId FOR JSON PATH), '[]')) AS 'StudentBatches'
		FROM Student S
		WHERE S.UserId = @userId AND S.IsDeleted = 0
		FOR JSON PATH;
	END

END
