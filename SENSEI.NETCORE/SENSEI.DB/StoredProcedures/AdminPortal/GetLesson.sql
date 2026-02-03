CREATE PROCEDURE [dbo].[GetLesson]
	@lessonId BIGINT = 0,
	@courseId BIGINT = 0
AS
BEGIN

	SET NOCOUNT ON;

	IF(@lessonId != 0)
	BEGIN
		SELECT Lesson.*,
			JSON_QUERY(ISNULL((SELECT * FROM Course WHERE Course.CourseId = Lesson.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
		FROM Lesson WHERE Lesson.IsDeleted = 0 AND Lesson.LessonId = @lessonId FOR JSON PATH;
	END
	ELSE IF(@courseId != 0)
	BEGIN
		SELECT Lesson.*,
			JSON_QUERY(ISNULL((SELECT * FROM Course WHERE Course.CourseId = Lesson.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
		FROM Lesson WHERE Lesson.IsDeleted = 0 AND Lesson.CourseId = @courseId FOR JSON PATH;
	END
	ELSE
	BEGIN
		SELECT Lesson.*,
			JSON_QUERY(ISNULL((SELECT * FROM Course WHERE Course.CourseId = Lesson.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
		FROM Lesson WHERE Lesson.IsDeleted = 0 FOR JSON PATH;
	END

END

