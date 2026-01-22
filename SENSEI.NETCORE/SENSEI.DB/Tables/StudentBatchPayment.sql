CREATE TABLE [dbo].[StudentBatch]
(
    [StudentBatchId] BIGINT NOT NULL IDENTITY,
    [BatchId] BIGINT NOT NULL,
    [StudentId] BIGINT NOT NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_StudentBatch] PRIMARY KEY CLUSTERED ([StudentBatchId])
);