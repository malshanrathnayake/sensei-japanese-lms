CREATE PROCEDURE [dbo].[GetCourse]
	@courseId BIGINT = 0
AS
BEGIN

	SET NOCOUNT ON;

	IF(@courseId = 0)
	BEGIN
		SELECT * FROM Course WHERE IsDeleted = 0 FOR JSON PATH;
	END
	ELSE
	BEGIN
		SELECT * FROM Course WHERE CourseId = @courseId AND IsDeleted = 0 FOR JSON PATH;
	END

END
