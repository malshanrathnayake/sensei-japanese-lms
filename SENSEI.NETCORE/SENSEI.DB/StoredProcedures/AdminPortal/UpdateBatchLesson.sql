CREATE PROCEDURE [dbo].[UpdateBatchLesson]
    @jsonString NVARCHAR(MAX) = '',
    @executionStatus BIT OUT,
    @primaryKey BIGINT OUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

        DECLARE @batchLessonId BIGINT;

        SELECT @batchLessonId = [BatchLessonId]
        FROM OPENJSON(@jsonString, '$')
        WITH (
            [BatchLessonId] BIGINT
        );

        IF (ISNULL(@batchLessonId, 0) = 0)
        BEGIN
            INSERT INTO [dbo].[BatchLesson]
                ([LessonId], [BatchId], [LessonDateTime], [RecordingUrl], [RecordingExpireDate], [IsDeleted])
            SELECT
                [LessonId], [BatchId], [LessonDateTime], [RecordingUrl], [RecordingExpireDate], ISNULL([IsDeleted], 0)
            FROM OPENJSON(@jsonString, '$')
            WITH (
                [LessonId] BIGINT,
                [BatchId] BIGINT,
                [LessonDateTime] DATETIME,
                [RecordingUrl] NVARCHAR(200),
                [RecordingExpireDate] DATETIME,
                [IsDeleted] BIT
            );

            SET @primaryKey = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE bl
            SET
                bl.[LessonId] = j.[LessonId],
                bl.[BatchId] = j.[BatchId],
                bl.[LessonDateTime] = j.[LessonDateTime],
                bl.[RecordingUrl] = j.[RecordingUrl],
                bl.[RecordingExpireDate] = j.[RecordingExpireDate],
                bl.[IsDeleted] = ISNULL(j.[IsDeleted], bl.[IsDeleted])
            FROM [dbo].[BatchLesson] bl
            INNER JOIN OPENJSON(@jsonString, '$')
            WITH (
                [BatchLessonId] BIGINT,
                [LessonId] BIGINT,
                [BatchId] BIGINT,
                [LessonDateTime] DATETIME,
                [RecordingUrl] NVARCHAR(200),
                [RecordingExpireDate] DATETIME,
                [IsDeleted] BIT
            ) AS j ON bl.[BatchLessonId] = j.[BatchLessonId]
            WHERE bl.[BatchLessonId] = @batchLessonId;

            SET @primaryKey = @batchLessonId;
        END

        COMMIT TRANSACTION;
        SET @executionStatus = 1;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @executionStatus = 0;
        THROW;
    END CATCH
END