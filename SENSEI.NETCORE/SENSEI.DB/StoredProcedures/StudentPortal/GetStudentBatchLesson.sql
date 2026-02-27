CREATE PROCEDURE [dbo].[GetStudentBatchLesson]
	@studentId BIGINT,
	@batchLessonId BIGINT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT BL.*,
		JSON_QUERY(ISNULL((SELECT Batch.* FROM Batch WHERE Batch.BatchId = BL.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
		JSON_QUERY(ISNULL((SELECT Lesson.* FROM Lesson WHERE Lesson.LessonId = BL.LessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Lesson'
	FROM BatchLesson BL
	INNER JOIN StudentBatch SB ON SB.BatchId = BL.BatchId
	WHERE BL.BatchLessonId = @batchLessonId 
		AND BL.IsDeleted = 0 
		AND SB.StudentId = @studentId 
		AND SB.IsDeleted = 0
	FOR JSON PATH;

END
