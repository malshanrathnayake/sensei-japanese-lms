CREATE TABLE [dbo].[BatchStudentLessonAccessRequest]
(
    [BatchStudentLessonAccessRequestId] BIGINT NOT NULL IDENTITY,
    [BatchStudentLessonAccessId] BIGINT NOT NULL,
    [RequestedDate] DATETIME NOT NULL,
    [RequestEndDate] DATETIME NOT NULL,
    [ApproveStatusEnum] BIT NOT NULL DEFAULT 0, -- 0 for pending, 1 for approved, 2 for rejected
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    [ChangeById] BIGINT NULL,
    [ChangeDate] DATETIME NULL,

    CONSTRAINT [PK_BatchStudentLessonAccessRequest] PRIMARY KEY CLUSTERED ([BatchStudentLessonAccessRequestId])
);