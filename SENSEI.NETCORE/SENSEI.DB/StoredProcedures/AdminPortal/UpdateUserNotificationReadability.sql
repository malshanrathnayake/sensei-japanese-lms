CREATE PROCEDURE [dbo].[UpdateUserNotificationReadability]
(
    @jsonString NVARCHAR(MAX) = '',
    @executionStatus BIT OUT,
    @primaryKey BIGINT OUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

        DECLARE @userNotificationId BIGINT, @userId BIGINT;

        SELECT @userNotificationId = [UserNotificationId], @userId = [UserId]
        FROM OPENJSON(@jsonString, '$')
        WITH (
            UserNotificationId BIGINT,
            UserId BIGINT
        ) AS JSONData;

        --UPDATE UserNotification SET IsRead = 1, ReadAt = GETDATE()
        --WHERE UserNotificationId = @userNotificationId;

        INSERT INTO UserNotificationRead([UserNotificationId], [UserId], [IsRead], [ReadAt])
        SELECT @userNotificationId, @userId, 1, GETDATE();

        COMMIT TRANSACTION;
        SET @executionStatus = 1;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @executionStatus = 0;
        THROW;
    END CATCH
END
