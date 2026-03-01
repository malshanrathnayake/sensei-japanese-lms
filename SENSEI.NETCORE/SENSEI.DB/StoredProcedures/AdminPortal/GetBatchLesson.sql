CREATE PROCEDURE [dbo].[GetBatchLesson]
	@batchLessonId BIGINT = 0,
	@batchId BIGINT = 0
AS
BEGIN
	
	SET NOCOUNT ON;

	IF(@batchLessonId != 0)
	BEGIN
		SELECT BatchLesson.*,
			JSON_QUERY(ISNULL((SELECT * FROM Batch WHERE Batch.BatchId = BatchLesson.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
			JSON_QUERY(ISNULL((SELECT * FROM Lesson WHERE Lesson.LessonId = BatchLesson.LessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Lesson',
			JSON_QUERY(ISNULL((SELECT * FROM BatchLessonReference WHERE BatchLessonReference.BatchLessonId = BatchLesson.BatchLessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'BatchLessonReferences'
		FROM BatchLesson WHERE BatchLesson.IsDeleted = 0 AND BatchLessonId = @batchLessonId FOR JSON PATH;
	END
	ELSE IF(@batchId != 0)
	BEGIN
		SELECT BatchLesson.*,
			JSON_QUERY(ISNULL((SELECT * FROM Batch WHERE Batch.BatchId = BatchLesson.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
			JSON_QUERY(ISNULL((SELECT * FROM Lesson WHERE Lesson.LessonId = BatchLesson.LessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Lesson',
			JSON_QUERY(ISNULL((SELECT * FROM BatchLessonReference WHERE BatchLessonReference.BatchLessonId = BatchLesson.BatchLessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'BatchLessonReferences'
		FROM BatchLesson WHERE BatchLesson.IsDeleted = 0 AND BatchLesson.BatchId = @batchId FOR JSON PATH;
	END
	ELSE
	BEGIN
		SELECT BatchLesson.*,
			JSON_QUERY(ISNULL((SELECT * FROM Batch WHERE Batch.BatchId = BatchLesson.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
			JSON_QUERY(ISNULL((SELECT * FROM Lesson WHERE Lesson.LessonId = BatchLesson.LessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Lesson',
			JSON_QUERY(ISNULL((SELECT * FROM BatchLessonReference WHERE BatchLessonReference.BatchLessonId = BatchLesson.BatchLessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'BatchLessonReferences'
		FROM BatchLesson WHERE BatchLesson.IsDeleted = 0 FOR JSON PATH;
	END

END
