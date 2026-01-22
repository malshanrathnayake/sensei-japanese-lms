CREATE TABLE [dbo].[Course]
(
    [CourseId] BIGINT NOT NULL IDENTITY,
    [CourseName] NVARCHAR(200) NOT NULL,
    [CourseCode] NVARCHAR(200) NOT NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED ([CourseId])
);