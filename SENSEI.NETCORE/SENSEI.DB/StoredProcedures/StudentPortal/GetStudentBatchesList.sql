CREATE PROCEDURE [dbo].[GetStudentBatchesList]
	@studentId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT SB.StudentBatchId, B.BatchName, C.CourseName
	FROM StudentBatch SB
	INNER JOIN Batch B ON B.BatchId = SB.BatchId
	INNER JOIN Course C ON C.CourseId = B.CourseId
	WHERE SB.StudentId = @studentId AND SB.IsDeleted = 0 AND B.IsDeleted = 0
	FOR JSON PATH;
END
