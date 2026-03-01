CREATE PROCEDURE [dbo].[GetStudentBatchById]
	@studentBatchId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	

	SELECT SB.*,
		JSON_QUERY(ISNULL((SELECT Batch.* FROM Batch WHERE Batch.BatchId = SB.BatchId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Batch'
	FROM StudentBatch SB WHERE SB.StudentBatchId = @studentBatchId AND SB.IsDeleted = 0 FOR JSON PATH;

END
