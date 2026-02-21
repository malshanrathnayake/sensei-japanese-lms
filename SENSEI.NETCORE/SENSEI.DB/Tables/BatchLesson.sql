CREATE TABLE [dbo].[BatchLesson]
(
    [BatchLessonId] BIGINT NOT NULL IDENTITY,
    [LessonId] BIGINT NOT NULL,
    [BatchId] BIGINT NOT NULL,
    [LessonDateTime] DATETIME NOT NULL,
    [RecordingUrl] NVARCHAR(200) NOT NULL,
    [RecordingExpireDate] DATETIME NOT NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_BatchLesson] PRIMARY KEY CLUSTERED ([BatchLessonId])
);