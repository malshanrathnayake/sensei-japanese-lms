CREATE PROCEDURE [dbo].[GetLesson]
	@lessonId BIGINT = 0,
	@courseId BIGINT = 0
AS
BEGIN

	SET NOCOUNT ON;

	IF(@lessonId != 0)
	BEGIN
		SELECT * FROM Lesson WHERE LessonId = @lessonId AND IsDeleted = 0 FOR JSON PATH;
	END
	ELSE IF(@courseId != 0)
	BEGIN
		SELECT * FROM Lesson WHERE CourseId = @courseId AND IsDeleted = 0 FOR JSON PATH;
	END
	ELSE
	BEGIN
		SELECT * FROM Lesson WHERE IsDeleted = 0 FOR JSON PATH;
	END

END

