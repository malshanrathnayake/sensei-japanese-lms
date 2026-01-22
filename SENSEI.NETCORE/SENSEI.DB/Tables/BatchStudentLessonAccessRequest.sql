CREATE TABLE [dbo].[BatchStudentLessonAccessRequest]
(
    [BatchStudentLessonAccessRequestId] BIGINT NOT NULL IDENTITY,
    [BatchStudentLessonAccessId] BIGINT NOT NULL,
    [RequestedDate] DATETIME NOT NULL,
    [RequestEndDate] DATETIME NOT NULL,
    [AdminApproved] BIT NOT NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_BatchStudentLessonAccessRequest] PRIMARY KEY CLUSTERED ([BatchStudentLessonAccessRequestId])
);