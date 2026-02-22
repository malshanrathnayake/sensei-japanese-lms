CREATE PROCEDURE [dbo].[SearchBatchLessons]
	@courseId BIGINT = 0,
	@batchId BIGINT = 0,
	@start INT = 0,
	@length INT = 10,
	@searchValue NVARCHAR(MAX),
	@sortColumn NVARCHAR(MAX),
	@sortDirection NVARCHAR(MAX),
	@count INT OUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT BL.*,
		JSON_QUERY(ISNULL((SELECT Batch.* FROM Batch WHERE Batch.IsDeleted = 0 AND Batch.BatchId = BL.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch',
		JSON_QUERY(ISNULL((SELECT Lesson.* FROM Lesson WHERE Lesson.IsDeleted = 0 AND Lesson.LessonId = BL.LessonId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Lesson'
	FROM BatchLesson BL
	INNER JOIN Batch B ON B.BatchId = BL.BatchId
	INNER JOIN Lesson LES ON LES.LessonId = BL.LessonId
	WHERE BL.IsDeleted = 0
		AND (@batchId = 0 OR BL.BatchId = @batchId)
		AND (@courseId = 0 OR B.CourseId = @courseId)
		AND (@searchValue = '' OR B.BatchName LIKE '%' + @searchValue + '%' OR LES.LessonName LIKE '%' + @searchValue + '%' OR CAST(BL.LessonDateTime AS DATE) LIKE '%' + @searchValue + '%' OR BL.RecordingUrl LIKE '%' + @searchValue + '%')
	ORDER BY 
		CASE WHEN @sortColumn = 'batch.batchName' AND @sortDirection = 'ASC' THEN B.BatchName END ASC,CASE WHEN @sortColumn = 'batch.batchName' AND @sortDirection = 'DESC' THEN B.BatchName END DESC,
		CASE WHEN @sortColumn = 'lesson.lessonName' AND @sortDirection = 'ASC' THEN LES.LessonName END ASC,CASE WHEN @sortColumn = 'lesson.lessonName' AND @sortDirection = 'DESC' THEN LES.LessonName END DESC,
		CASE WHEN @sortColumn = 'lessonDateTime' AND @sortDirection = 'ASC' THEN BL.LessonDateTime END ASC,CASE WHEN @sortColumn = 'lessonDateTime' AND @sortDirection = 'DESC' THEN BL.LessonDateTime END DESC,
		CASE WHEN @sortColumn = 'recordingUrl' AND @sortDirection = 'ASC' THEN BL.RecordingUrl END ASC,CASE WHEN @sortColumn = 'recordingUrl' AND @sortDirection = 'DESC' THEN BL.RecordingUrl END DESC
	OFFSET @start ROWS
	FETCH NEXT @length ROWS ONLY
	FOR JSON PATH;


	SELECT @count = COUNT(*)
	FROM BatchLesson BL
	INNER JOIN Batch B ON B.BatchId = BL.BatchId
	INNER JOIN Lesson LES ON LES.LessonId = BL.LessonId
	WHERE BL.IsDeleted = 0
		AND (@batchId = 0 OR BL.BatchId = @batchId)
		AND (@courseId = 0 OR B.CourseId = @courseId)
		AND (@searchValue = '' OR B.BatchName LIKE '%' + @searchValue + '%' OR LES.LessonName LIKE '%' + @searchValue + '%' OR CAST(BL.LessonDateTime AS DATE) LIKE '%' + @searchValue + '%' OR BL.RecordingUrl LIKE '%' + @searchValue + '%')

END
