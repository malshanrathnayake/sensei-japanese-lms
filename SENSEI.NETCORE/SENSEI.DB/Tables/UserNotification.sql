CREATE TABLE [dbo].[UserNotification]
(
	[UserNotificationId] BIGINT NOT NULL IDENTITY,
	[UserId] BIGINT NULL,
	[UserTypeEnum] INT NULL,
	[NotificationType] NVARCHAR(100) NULL,
	[Message] NVARCHAR(MAX) NULL,
	[IsRead] BIT NOT NULL DEFAULT 0,
	[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[ReadAt] DATETIME NULL,
	[Icon] NVARCHAR(100) NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[BatchId] BIGINT NULL,
	[CourseId] BIGINT NULL,

	CONSTRAINT [PK_UserNotification_UserNotificationId] PRIMARY KEY CLUSTERED ([UserNotificationId]),
	CONSTRAINT [FK_UserNotification_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]),
	CONSTRAINT [FK_UserNotification_Batch_BatchId] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[Batch] ([BatchId]),
	CONSTRAINT [FK_UserNotification_Course_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Course] ([CourseId])
)
