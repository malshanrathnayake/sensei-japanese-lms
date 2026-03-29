CREATE PROCEDURE [dbo].[UpdateBatchLesson]
    @jsonString NVARCHAR(MAX) = '',
    @executionStatus BIT OUT,
    @primaryKey BIGINT OUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

        DECLARE @batchLessonId BIGINT, @batchId BIGINT, @lessonDateTime DATETIME;

        SELECT @batchLessonId = [BatchLessonId], @batchId = [BatchId], @lessonDateTime = [LessonDateTime]
        FROM OPENJSON(@jsonString, '$')
        WITH (
            [BatchLessonId] BIGINT,
            [BatchId] BIGINT,
            [LessonDateTime] DATETIMEOFFSET
        );

        IF (ISNULL(@batchLessonId, 0) = 0)
        BEGIN
            INSERT INTO [dbo].[BatchLesson]
                ([LessonId], [BatchId], [LessonDateTime], [RecordingUrl], [RecordingExpireDate], [IsDeleted], [Description])
            SELECT
                [LessonId], [BatchId], [LessonDateTime], [RecordingUrl], [RecordingExpireDate], ISNULL([IsDeleted], 0), [Description]
            FROM OPENJSON(@jsonString, '$')
            WITH (
                [LessonId] BIGINT,
                [BatchId] BIGINT,
                [LessonDateTime] DATETIME,
                [RecordingUrl] NVARCHAR(200),
                [RecordingExpireDate] DATETIME,
                [IsDeleted] BIT,
                [Description] NVARCHAR(200)
            );

            SET @primaryKey = SCOPE_IDENTITY();

            -- will insert access for all students of the batch, who has paid the fee for the batch
            INSERT INTO BatchStudentLessonAccess([BatchLessonId], StudentId, [HasAccess])
            SELECT  @primaryKey, sb.StudentId, 1
            FROM StudentBatch sb
            WHERE sb.BatchId = @batchId
            AND sb.StudentBatchId IN (SELECT StudentBatchId FROM StudentBatchPayment WHERE StudentBatchId = sb.StudentBatchId AND MONTH(PaymentMonth) = MONTH(@lessonDateTime) AND YEAR(PaymentMonth) = YEAR(@lessonDateTime))

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
                bl.[IsDeleted] = ISNULL(j.[IsDeleted], bl.[IsDeleted]),
                bl.[Description] = j.[Description]
            FROM [dbo].[BatchLesson] bl
            INNER JOIN OPENJSON(@jsonString, '$')
            WITH (
                [BatchLessonId] BIGINT,
                [LessonId] BIGINT,
                [BatchId] BIGINT,
                [LessonDateTime] DATETIME,
                [RecordingUrl] NVARCHAR(200),
                [RecordingExpireDate] DATETIME,
                [IsDeleted] BIT,
                [Description] NVARCHAR(200)
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