CREATE TABLE [dbo].[Lesson]
(
    [LessonId] BIGINT NOT NULL IDENTITY,
    [CourseId] BIGINT NOT NULL,
    [LessonName] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Lesson] PRIMARY KEY CLUSTERED ([LessonId])
);