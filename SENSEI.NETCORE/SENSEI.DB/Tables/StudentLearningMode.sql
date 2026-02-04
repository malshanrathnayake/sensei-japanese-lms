CREATE TABLE [dbo].[StudentLearningMode]
(
	[StudentLearningModeId] INT NOT NULL IDENTITY,
	[LearningModeName] NVARCHAR(100) NOT NULL,

	CONSTRAINT [PK_StudentLearningMode] PRIMARY KEY CLUSTERED ([StudentLearningModeId])
)
