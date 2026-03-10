CREATE PROCEDURE [dbo].[UpdateBatchLessonReference]
    @jsonString NVARCHAR(MAX) = '',
    @executionStatus BIT OUT,
    @primaryKey BIGINT OUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

        INSERT INTO BatchLessonReference([BatchLessonId], [ReferenceUrl], [Description])
        SELECT [BatchLessonId], [ReferenceUrl], [Description]
        FROM OPENJSON(@jsonString, '$')
        WITH (
            [BatchLessonId] BIGINT,
            [ReferenceUrl] NVARCHAR(MAX),
            [Description] NVARCHAR(MAX)
        ) AS BatchLessonReferenceData

        COMMIT TRANSACTION;
        SET @executionStatus = 1;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @executionStatus = 0;
        THROW;
    END CATCH
END