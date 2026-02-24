CREATE PROCEDURE [dbo].[SearchStudentBatches]
	@studentId BIGINT,
	@start INT = 0,
	@length INT = 10,
	@searchValue NVARCHAR(MAX) = '',
	@sortColumn NVARCHAR(MAX) = 'batchName',
	@sortDirection NVARCHAR(MAX) = 'ASC',
	@count INT OUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT B.*,
		JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.CourseId = B.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
	FROM Batch B
	INNER JOIN StudentBatch SB ON SB.BatchId = B.BatchId
	WHERE B.IsDeleted = 0 
		AND SB.StudentId = @studentId 
		AND SB.IsDeleted = 0
		AND (@searchValue = '' OR B.BatchName LIKE '%' + @searchValue + '%' OR CAST(B.BatchStartDate AS DATE) LIKE '%' + @searchValue + '%' OR CAST(B.BatchEndDate AS DATE) LIKE '%' + @searchValue + '%')
	ORDER BY 
		CASE WHEN @sortColumn = 'batchName' AND @sortDirection = 'ASC' THEN B.BatchName END ASC,CASE WHEN @sortColumn = 'batchName' AND @sortDirection = 'DESC' THEN B.BatchName END DESC,
		CASE WHEN @sortColumn = 'batchStartDate' AND @sortDirection = 'ASC' THEN B.BatchStartDate END ASC,CASE WHEN @sortColumn = 'batchStartDate' AND @sortDirection = 'DESC' THEN B.BatchStartDate END DESC,
		CASE WHEN @sortColumn = 'batchEndDate' AND @sortDirection = 'ASC' THEN B.BatchEndDate END ASC,CASE WHEN @sortColumn = 'batchEndDate' AND @sortDirection = 'DESC' THEN B.BatchEndDate END DESC
	OFFSET @start ROWS
	FETCH NEXT @length ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(*)
	FROM Batch B
	INNER JOIN StudentBatch SB ON SB.BatchId = B.BatchId
	WHERE B.IsDeleted = 0 
		AND SB.StudentId = @studentId 
		AND SB.IsDeleted = 0
		AND (@searchValue = '' OR B.BatchName LIKE '%' + @searchValue + '%' OR CAST(B.BatchStartDate AS DATE) LIKE '%' + @searchValue + '%' OR CAST(B.BatchEndDate AS DATE) LIKE '%' + @searchValue + '%');

END
