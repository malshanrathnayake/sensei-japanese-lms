CREATE PROCEDURE [dbo].[GetUserNotificationForUser]
	@UserId BIGINT = 0
AS
BEGIN

	SET NOCOUNT ON;

	SELECT UN.* 
	FROM UserNotification UN
	WHERE 
		UN.IsDeleted = 0
		AND (UN.UserId IS NULL OR UN.UserId = @UserId)
		AND (UN.UserTypeEnum = (SELECT UserTypeEnum FROM [User] WHERE UserId = @UserId))
		AND (UN.BatchId IS NULL OR UN.BatchId IN (SELECT SB.BatchId FROM StudentBatch SB INNER JOIN [Student] STD ON STD.StudentId = SB.StudentId AND STD.UserId = @UserId))
		AND (UN.CourseId IS NULL OR UN.CourseId IN (SELECT C.CourseId FROM Course C INNER JOIN Batch B ON B.CourseId = C.CourseId INNER JOIN StudentBatch SB ON SB.BatchId = B.BatchId INNER JOIN [Student] STD ON STD.StudentId = SB.StudentId AND STD.UserId = @UserId))
		AND (UN.UserNotificationId NOT IN (SELECT UserNotificationId FROM UserNotificationRead WHERE UserId = @UserId))
		ORDER BY UN.UserNotificationId DESC
	FOR JSON PATH

END
