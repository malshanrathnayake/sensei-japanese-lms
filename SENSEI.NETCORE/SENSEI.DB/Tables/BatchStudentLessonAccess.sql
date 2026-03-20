CREATE TABLE [dbo].[BatchStudentLessonAccess]
(
    [BatchStudentLessonAccessId] BIGINT NOT NULL IDENTITY,
    [BatchLessonId] BIGINT NOT NULL,
    [StudentId] BIGINT NOT NULL,
    [HasAccess] BIT NOT NULL DEFAULT 0,
    [Feedback] NVARCHAR(MAX) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [Rating] INT NULL

    CONSTRAINT [PK_BatchStudentLessonAccess] PRIMARY KEY CLUSTERED ([BatchStudentLessonAccessId])

);