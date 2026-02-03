CREATE PROCEDURE [dbo].[GetBatch]
	@batchId BIGINT = 0,
	@courseId BIGINT = 0
AS
BEGIN

	SET NOCOUNT ON;

	IF(@batchId != 0)
	BEGIN
		SELECT Batch.*,
			JSON_QUERY(ISNULL((SELECT * FROM Course WHERE Course.CourseId = Batch.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
		FROM Batch WHERE Batch.IsDeleted = 0 AND BatchId = @batchId	 FOR JSON PATH;
	END
	ELSE IF(@courseId != 0)
	BEGIN
		SELECT Batch.*,
			JSON_QUERY(ISNULL((SELECT * FROM Course WHERE Course.CourseId = Batch.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
		FROM Batch WHERE Batch.IsDeleted = 0 AND Batch.CourseId = @courseId FOR JSON PATH;
	END
	ELSE
	BEGIN
		SELECT Batch.*,
			JSON_QUERY(ISNULL((SELECT * FROM Course WHERE Course.CourseId = Batch.CourseId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Course'
		FROM Batch WHERE Batch.IsDeleted = 0 FOR JSON PATH;
	END

END