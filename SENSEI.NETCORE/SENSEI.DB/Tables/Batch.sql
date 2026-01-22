CREATE TABLE [dbo].[Batch]
(
    [BatchId] BIGINT NOT NULL IDENTITY,
    [CourseId] BIGINT NOT NULL,
    [BatchName] NVARCHAR(200) NOT NULL,
    [BatchStartDate] DATETIME NOT NULL,
    [BatchEndDate] DATETIME NOT NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_Batch] PRIMARY KEY CLUSTERED ([BatchId])
);