CREATE PROCEDURE [dbo].[UpdateUserNotification]
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

        INSERT INTO UserNotification([UserId], [UserTypeEnum], [NotificationType], [Message], [Icon], [BatchId], [CourseId])
        SELECT [UserId], [UserTypeEnum], [NotificationType], [Message], [Icon], [BatchId], [CourseId]
        FROM OPENJSON(@jsonString, '$')
        WITH (
            [UserId] BIGINT,
            [UserTypeEnum] INT,
            [NotificationType] NVARCHAR(100),
            [Message] NVARCHAR(MAX),
            [Icon] NVARCHAR(100),
            [BatchId] BIGINT,
            [CourseId] BIGINT
        ) AS JSONData;

        COMMIT TRANSACTION;
        SET @executionStatus = 1;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @executionStatus = 0;
        THROW;
    END CATCH
END