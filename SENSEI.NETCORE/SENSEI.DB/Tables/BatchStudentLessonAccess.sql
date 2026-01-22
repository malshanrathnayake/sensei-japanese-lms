CREATE TABLE [dbo].[BatchStudentLessonAccess]
(
    [BatchStudentLessonAccessId] BIGINT NOT NULL IDENTITY,
    [BatchLessonId] BIGINT NOT NULL,
    [StudentId] BIGINT NOT NULL,
    [HasAccess] BIT NOT NULL,
    [Feedback] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_BatchStudentLessonAccess] PRIMARY KEY CLUSTERED ([BatchStudentLessonAccessId])
);