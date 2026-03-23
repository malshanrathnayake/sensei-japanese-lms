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
        );

        INSERT INTO UserNotificationRead([UserNotificationId], [UserId], [IsRead], [ReadAt])
        VALUES(@userNotificationId, @userId, 1, GETDATE());

        COMMIT TRANSACTION;
        SET @executionStatus = 1;
        SET @primaryKey = 1;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @executionStatus = 0;
        SET @primaryKey = 0;
        THROW;
    END CATCH
END
GO
