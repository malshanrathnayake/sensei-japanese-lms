CREATE TABLE [dbo].[UserNotificationRead]
(
	[UserNotificationReadId] BIGINT NOT NULL IDENTITY,
	[UserNotificationId] BIGINT NOT NULL,
	[UserId] BIGINT NOT NULL,
	[IsRead] BIT NOT NULL DEFAULT 0,
	[ReadAt] DATETIME NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,

	CONSTRAINT [PK_UserNotificationRead_UserNotificationReadId] PRIMARY KEY CLUSTERED ([UserNotificationReadId]),
	CONSTRAINT [FK_UserNotificationRead_UserNotificationId] FOREIGN KEY ([UserNotificationId]) REFERENCES [dbo].[UserNotification]([UserNotificationId])
)
