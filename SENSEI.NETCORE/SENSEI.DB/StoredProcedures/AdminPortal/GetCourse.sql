CREATE PROCEDURE [dbo].[GetCourse]
	@courseId BIGINT = 0
AS
BEGIN

	SELECT * FROM Course WHERE CourseId = @courseId AND IsDeleted = 0 FOR JSON PATH;

END
