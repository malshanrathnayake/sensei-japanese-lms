CREATE TABLE [dbo].[BatchLessonReference]
(
	[BatchLessonReferenceId] BIGINT NOT NULL IDENTITY,
	[BatchLessonId] BIGINT NOT NULL,
	[ReferenceUrl] NVARCHAR(MAX) NOT NULL,
	[Description] NVARCHAR(MAX) NULL,

	CONSTRAINT [PK_BatchLessonReference_BatchLessonReferenceId] PRIMARY KEY CLUSTERED ([BatchLessonReferenceId])
)
