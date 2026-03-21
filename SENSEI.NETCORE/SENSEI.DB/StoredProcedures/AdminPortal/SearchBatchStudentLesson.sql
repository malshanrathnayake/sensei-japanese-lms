CREATE PROCEDURE [dbo].[SearchBatchStudentLesson]
	@courseId BIGINT = 0,
	@batchId BIGINT = 0,
	@indexNumber NVARCHAR(500) = '',
	@start INT = 0,
	@length INT = 10,
	@searchValue NVARCHAR(MAX),
	@sortColumn NVARCHAR(MAX),
	@sortDirection NVARCHAR(MAX),
	@count INT OUT
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
	INNER JOIN BatchStudentLessonAccess BSLA on BSLA.BatchStudentLessonAccessId = BSLAR.BatchStudentLessonAccessId
	INNER JOIN Student ST ON ST.StudentId = BSLA.StudentId
	INNER JOIN BatchLesson BL ON BL.BatchLessonId = BSLA.BatchLessonId
	INNER JOIN Lesson Les ON Les.LessonId = BL.LessonId
	INNER JOIN Batch B ON B.BatchId = BL.BatchId
	WHERE
		BSLAR.IsDeleted = 0
		AND (@courseId = 0 OR Les.CourseId = @courseId)
		AND (@batchId = 0 OR BL.BatchId = @batchId)
		AND (@indexNumber = '' OR ST.IndexNumber = @indexNumber)
		AND (@searchValue = '' OR ST.IndexNumber LIKE '%' + @searchValue + '&' OR ST.FirstName LIKE '%' + @searchValue + '&' OR ST.LastName LIKE '%' + @searchValue + '&'
			OR ST.MiddleName LIKE '%' + @searchValue + '&' OR Les.LessonName LIKE '%' + @searchValue + '&' OR BL.[Description] LIKE '%' + @searchValue + '&'
			OR CAST(BSLAR.RequestedDate AS DATE) LIKE '%' + @searchValue + '&' OR B.BatchName LIKE '%' + @searchValue + '&')
	ORDER BY 
		CASE WHEN @sortColumn = 'batchName' AND @sortDirection = 'ASC' THEN B.BatchName END ASC,CASE WHEN @sortColumn = 'batchName' AND @sortDirection = 'DESC' THEN B.BatchName END DESC,
		CASE WHEN @sortColumn = 'lessonName' AND @sortDirection = 'ASC' THEN Les.LessonName END ASC,CASE WHEN @sortColumn = 'lessonName' AND @sortDirection = 'DESC' THEN Les.LessonName END DESC,
		CASE WHEN @sortColumn = 'description' AND @sortDirection = 'ASC' THEN BL.[Description] END ASC,CASE WHEN @sortColumn = 'description' AND @sortDirection = 'DESC' THEN BL.[Description] END DESC,
		CASE WHEN @sortColumn = 'indexNumber' AND @sortDirection = 'ASC' THEN ST.IndexNumber END ASC,CASE WHEN @sortColumn = 'indexNumber' AND @sortDirection = 'DESC' THEN ST.IndexNumber END DESC,
		CASE WHEN @sortColumn = 'studentPopulatedName' AND @sortDirection = 'ASC' THEN ST.FirstName END ASC,CASE WHEN @sortColumn = 'studentPopulatedName' AND @sortDirection = 'DESC' THEN ST.FirstName END DESC,
		CASE WHEN @sortColumn = 'requestedDate' AND @sortDirection = 'ASC' THEN BSLAR.RequestedDate END ASC,CASE WHEN @sortColumn = 'requestedDate' AND @sortDirection = 'DESC' THEN BSLAR.RequestedDate END DESC
	OFFSET @start ROWS
	FETCH NEXT (CASE WHEN @length = -1 THEN 2147483647 ELSE @length END) ROWS ONLY
	FOR JSON PATH;


	--count
	SELECT @count = COUNT(*)
	FROM BatchStudentLessonAccessRequest BSLAR
	INNER JOIN BatchStudentLessonAccess BSLA on BSLA.BatchStudentLessonAccessId = BSLAR.BatchStudentLessonAccessId
	INNER JOIN Student ST ON ST.StudentId = BSLA.StudentId
	INNER JOIN BatchLesson BL ON BL.BatchLessonId = BSLA.BatchLessonId
	INNER JOIN Lesson Les ON Les.LessonId = BL.LessonId
	INNER JOIN Batch B ON B.BatchId = BL.BatchId
	WHERE
		BSLAR.IsDeleted = 0
		AND (@courseId = 0 OR Les.CourseId = @courseId)
		AND (@batchId = 0 OR BL.BatchId = @batchId)
		AND (@indexNumber = '' OR ST.IndexNumber = @indexNumber)
		AND (@searchValue = '' OR ST.IndexNumber LIKE '%' + @searchValue + '&' OR ST.FirstName LIKE '%' + @searchValue + '&' OR ST.LastName LIKE '%' + @searchValue + '&'
			OR ST.MiddleName LIKE '%' + @searchValue + '&' OR Les.LessonName LIKE '%' + @searchValue + '&' OR BL.[Description] LIKE '%' + @searchValue + '&'
			OR CAST(BSLAR.RequestedDate AS DATE) LIKE '%' + @searchValue + '&' OR B.BatchName LIKE '%' + @searchValue + '&')

END
