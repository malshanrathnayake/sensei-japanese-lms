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

	CONSTRAINT [PK_UserNotification_UserNotificationId] PRIMARY KEY CLUSTERED ([UserNotificationId])
)
