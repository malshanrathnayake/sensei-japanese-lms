CREATE PROCEDURE [dbo].[SearchStudentBatchLessons]
	@studentId BIGINT,
	@batchId BIGINT = 0,
	@start INT = 0,
	@length INT = 2147483647 ,
	@searchValue NVARCHAR(MAX) = '',
	@sortColumn NVARCHAR(MAX) = 'lessonDateTime',
	@sortDirection NVARCHAR(MAX) = 'DESC',
	@count INT OUT
AS
BEGIN

	SET NOCOUNT ON;

	IF(@length != -1)
	BEGIN

		SELECT BL.*,
			JSON_QUERY(ISNULL((SELECT Batch.* FROM Batch WHERE Batch.BatchId = BL.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
			JSON_QUERY(ISNULL((SELECT Lesson.*,
				JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.CourseId = Lesson.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
			FROM Lesson WHERE Lesson.LessonId = BL.LessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Lesson',
			JSON_QUERY(ISNULL((SELECT BSLA.*,
				JSON_QUERY(ISNULL((SELECT BatchStudentLessonAccessRequest.* FROM BatchStudentLessonAccessRequest WHERE BatchStudentLessonAccessRequest.BatchStudentLessonAccessId = BSLA.BatchStudentLessonAccessId FOR JSON PATH), '[]')) AS 'BatchStudentLessonAccessRequests'
			FROM BatchStudentLessonAccess BSLA WHERE BSLA.BatchLessonId = BL.BatchLessonId AND BSLA.StudentId = @studentId FOR JSON PATH), '[]')) AS 'BatchStudentLessonAccesses',
			JSON_QUERY(ISNULL((SELECT BatchLessonReference.* FROM BatchLessonReference WHERE BatchLessonReference.BatchLessonId = BL.BatchLessonId FOR JSON PATH), '[]')) AS 'BatchLessonReferences',
			JSON_QUERY(ISNULL((SELECT StudentBatchLessonView.* FROM StudentBatchLessonView WHERE StudentBatchLessonView.BatchLessonId = BL.BatchLessonId AND StudentBatchLessonView.StudentId = @studentId FOR JSON PATH), '[]')) AS 'StudentBatchLessonViews'
		FROM BatchLesson BL
		INNER JOIN Batch B ON B.BatchId = BL.BatchId
		INNER JOIN Lesson LES ON LES.LessonId = BL.LessonId
		INNER JOIN StudentBatch SB ON SB.BatchId = B.BatchId
		WHERE BL.IsDeleted = 0 
			AND SB.StudentId = @studentId 
			AND SB.IsDeleted = 0
			AND (@batchId = 0 OR BL.BatchId = @batchId)
			AND (@searchValue = '' OR B.BatchName LIKE '%' + @searchValue + '%' OR LES.LessonName LIKE '%' + @searchValue + '%' OR CAST(BL.LessonDateTime AS DATE) LIKE '%' + @searchValue + '%')
		ORDER BY 
			CASE WHEN @sortColumn = 'batch.batchName' AND @sortDirection = 'ASC' THEN B.BatchName END ASC,CASE WHEN @sortColumn = 'batch.batchName' AND @sortDirection = 'DESC' THEN B.BatchName END DESC,
			CASE WHEN @sortColumn = 'lesson.lessonName' AND @sortDirection = 'ASC' THEN LES.LessonName END ASC,CASE WHEN @sortColumn = 'lesson.lessonName' AND @sortDirection = 'DESC' THEN LES.LessonName END DESC,
			CASE WHEN @sortColumn = 'lessonDateTime' AND @sortDirection = 'ASC' THEN BL.LessonDateTime END ASC,CASE WHEN @sortColumn = 'lessonDateTime' AND @sortDirection = 'DESC' THEN BL.LessonDateTime END DESC
		OFFSET @start ROWS
		FETCH NEXT @length ROWS ONLY
		FOR JSON PATH;

	END
	ELSE
	BEGIN

		SELECT BL.*,
			JSON_QUERY(ISNULL((SELECT Batch.* FROM Batch WHERE Batch.BatchId = BL.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
			JSON_QUERY(ISNULL((SELECT Lesson.*,
				JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.CourseId = Lesson.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
			FROM Lesson WHERE Lesson.LessonId = BL.LessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Lesson',
			JSON_QUERY(ISNULL((SELECT BSLA.*,
				JSON_QUERY(ISNULL((SELECT BatchStudentLessonAccessRequest.* FROM BatchStudentLessonAccessRequest WHERE BatchStudentLessonAccessRequest.BatchStudentLessonAccessId = BSLA.BatchStudentLessonAccessId FOR JSON PATH), '[]')) AS 'BatchStudentLessonAccessRequests'
			FROM BatchStudentLessonAccess BSLA WHERE BSLA.BatchLessonId = BL.BatchLessonId AND BSLA.StudentId = @studentId FOR JSON PATH), '[]')) AS 'BatchStudentLessonAccesses',
			JSON_QUERY(ISNULL((SELECT BatchLessonReference.* FROM BatchLessonReference WHERE BatchLessonReference.BatchLessonId = BL.BatchLessonId FOR JSON PATH), '[]')) AS 'BatchLessonReferences',
			JSON_QUERY(ISNULL((SELECT StudentBatchLessonView.* FROM StudentBatchLessonView WHERE StudentBatchLessonView.BatchLessonId = BL.BatchLessonId AND StudentBatchLessonView.StudentId = @studentId FOR JSON PATH), '[]')) AS 'StudentBatchLessonViews'
		FROM BatchLesson BL
		INNER JOIN Batch B ON B.BatchId = BL.BatchId
		INNER JOIN Lesson LES ON LES.LessonId = BL.LessonId
		INNER JOIN StudentBatch SB ON SB.BatchId = B.BatchId
		WHERE BL.IsDeleted = 0 
			AND SB.StudentId = @studentId 
			AND SB.IsDeleted = 0
			AND (@batchId = 0 OR BL.BatchId = @batchId)
			AND (@searchValue = '' OR B.BatchName LIKE '%' + @searchValue + '%' OR LES.LessonName LIKE '%' + @searchValue + '%' OR CAST(BL.LessonDateTime AS DATE) LIKE '%' + @searchValue + '%')
		ORDER BY 
			CASE WHEN @sortColumn = 'batch.batchName' AND @sortDirection = 'ASC' THEN B.BatchName END ASC,CASE WHEN @sortColumn = 'batch.batchName' AND @sortDirection = 'DESC' THEN B.BatchName END DESC,
			CASE WHEN @sortColumn = 'lesson.lessonName' AND @sortDirection = 'ASC' THEN LES.LessonName END ASC,CASE WHEN @sortColumn = 'lesson.lessonName' AND @sortDirection = 'DESC' THEN LES.LessonName END DESC,
			CASE WHEN @sortColumn = 'lessonDateTime' AND @sortDirection = 'ASC' THEN BL.LessonDateTime END ASC,CASE WHEN @sortColumn = 'lessonDateTime' AND @sortDirection = 'DESC' THEN BL.LessonDateTime END DESC
		FOR JSON PATH;

	END

	

	SELECT @count = COUNT(*)
	FROM BatchLesson BL
	INNER JOIN Batch B ON B.BatchId = BL.BatchId
	INNER JOIN Lesson LES ON LES.LessonId = BL.LessonId
	INNER JOIN StudentBatch SB ON SB.BatchId = B.BatchId
	WHERE BL.IsDeleted = 0 
		AND SB.StudentId = @studentId 
		AND SB.IsDeleted = 0
		AND (@batchId = 0 OR BL.BatchId = @batchId)
		AND (@searchValue = '' OR B.BatchName LIKE '%' + @searchValue + '%' OR LES.LessonName LIKE '%' + @searchValue + '%' OR CAST(BL.LessonDateTime AS DATE) LIKE '%' + @searchValue + '%');

END
