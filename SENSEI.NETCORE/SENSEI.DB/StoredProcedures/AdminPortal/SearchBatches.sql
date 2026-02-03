CREATE PROCEDURE [dbo].[SearchBatches]
	@courseId BIGINT = 0,
	@start INT = 0,
	@length INT = 10,
	@searchValue NVARCHAR(MAX),
	@sortColumn NVARCHAR(MAX),
	@sortDirection NVARCHAR(MAX),
	@count INT OUT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT BTC.*,
		JSON_QUERY(ISNULL((SELECT Course.* FROM Course WHERE Course.IsDeleted = 0 AND Course.CourseId = BTC.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
	FROM Batch BTC
	WHERE BTC.IsDeleted = 0
		AND (@courseId = 0 OR BTC.CourseId = @courseId)
		AND (@searchValue = '' OR BTC.BatchName LIKE '%' + @searchValue + '%' OR CAST(BTC.BatchStartDate AS DATE) LIKE '%' + @searchValue + '%' OR CAST(BTC.BatchEndDate AS DATE) LIKE '%' + @searchValue + '%') 
	ORDER BY 
		CASE WHEN @sortColumn = 'batchName' AND @sortDirection = 'ASC' THEN BTC.BatchName END ASC,CASE WHEN @sortColumn = 'batchName' AND @sortDirection = 'DESC' THEN BTC.BatchName END DESC,
		CASE WHEN @sortColumn = 'batchStartDate' AND @sortDirection = 'ASC' THEN BTC.BatchStartDate END ASC,CASE WHEN @sortColumn = 'batchStartDate' AND @sortDirection = 'DESC' THEN BTC.BatchStartDate END DESC,
		CASE WHEN @sortColumn = 'batchEndDate' AND @sortDirection = 'ASC' THEN BTC.BatchEndDate END ASC,CASE WHEN @sortColumn = 'batchEndDate' AND @sortDirection = 'DESC' THEN BTC.BatchEndDate END DESC
	OFFSET @start ROWS
	FETCH NEXT @length ROWS ONLY
	FOR JSON PATH;

	SELECT @count = COUNT(*)
	FROM Batch BTC
	WHERE BTC.IsDeleted = 0
		AND (@courseId = 0 OR BTC.CourseId = @courseId)
		AND (@searchValue = '' OR BTC.BatchName LIKE '%' + @searchValue + '%' OR CAST(BTC.BatchStartDate AS DATE) LIKE '%' + @searchValue + '%' OR CAST(BTC.BatchEndDate AS DATE) LIKE '%' + @searchValue + '%') 

END