CREATE TABLE [dbo].[StudentBatchPayment]
(
    [StudentBatchPaymentId] BIGINT NOT NULL IDENTITY,
    [StudentBatchId] BIGINT NOT NULL,
    [PaymentMonth] DATETIME NOT NULL,
    [Amount] DECIMAL(18, 2) NOT NULL,
    [PaymentDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [SlipUrl] NVARCHAR(MAX) NULL,
    [IsApproved] BIT NOT NULL DEFAULT 0,
    [ApprovedById] BIGINT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [IsRejected] BIT NOT NULL DEFAULT 0,
    [ChangeDateTIme] DATETIME NULL DEFAULT GETUTCDATE(),
    [LessonId] BIGINT NULL,
    [ReferenceNumber] NVARCHAR(100) NULL,
    [Remarks] NVARCHAR(500) NULL,

    CONSTRAINT [PK_StudentBatchPayment] PRIMARY KEY CLUSTERED ([StudentBatchPaymentId]),
    CONSTRAINT [FK_StudentBatchPayment_StudentBatchId] FOREIGN KEY ([StudentBatchId]) REFERENCES [StudentBatch]([StudentBatchId])
);
