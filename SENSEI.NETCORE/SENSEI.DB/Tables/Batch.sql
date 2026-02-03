CREATE TABLE [dbo].[Batch]
(
    [BatchId] BIGINT NOT NULL IDENTITY,
    [CourseId] BIGINT NOT NULL,
    [BatchName] NVARCHAR(200) NOT NULL,
    [BatchStartDate] DATETIME NOT NULL,
    [BatchEndDate] DATETIME NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [PK_Batch_BatchId] PRIMARY KEY CLUSTERED ([BatchId]),
    CONSTRAINT [FK_Batch_Course_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Course]([CourseId])
);