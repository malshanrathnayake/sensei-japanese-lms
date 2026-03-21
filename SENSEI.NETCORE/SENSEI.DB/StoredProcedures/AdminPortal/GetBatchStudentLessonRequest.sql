CREATE PROCEDURE [dbo].[GetBatchStudentLessonRequest]
	@batchStudentLessonRequestId BIGINT = 0
AS
BEGIN

	SET NOCOUNT ON;

	SELECT BSLAR.*,
		JSON_QUERY(ISNULL((SELECT BatchStudentLessonAccess.*,
			JSON_QUERY(ISNULL((SELECT Student.* FROM Student WHERE Student.IsDeleted = 0 AND Student.StudentId = BatchStudentLessonAccess.StudentId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Student',
			JSON_QUERY(ISNULL((SELECT BatchLesson.*,
				JSON_QUERY(ISNULL((SELECT Lesson.*,
					JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.IsDeleted = 0 AND Course.CourseId = Lesson.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
				FROM Lesson WHERE Lesson.IsDeleted = 0 AND Lesson.LessonId = BatchLesson.LessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Lesson',
				JSON_QUERY(ISNULL((SELECT Batch.*,
					JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.IsDeleted = 0 AND Course.CourseId = Batch.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
				FROM Batch WHERE Batch.IsDeleted = 0 AND Batch.BatchId = BatchLesson.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch'
			FROM BatchLesson WHERE BatchLesson.IsDeleted = 0 AND BatchLesson.BatchLessonId = BatchStudentLessonAccess.BatchLessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'BatchLesson'
		FROM BatchStudentLessonAccess WHERE BatchStudentLessonAccess.IsDeleted = 0 AND BatchStudentLessonAccess.BatchStudentLessonAccessId = BSLAR.BatchStudentLessonAccessId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'BatchStudentLessonAccess'
	FROM BatchStudentLessonAccessRequest BSLAR
	WHERE BSLAR.BatchStudentLessonAccessRequestId = @batchStudentLessonRequestId AND IsDeleted = 0 FOR JSON PATH

END
