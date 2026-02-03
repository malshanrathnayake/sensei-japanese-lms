CREATE TABLE [dbo].[StudentRegistration]
(
	[StudentRegistrationId] BIGINT NOT NULL IDENTITY,
	[Email] NVARCHAR(256) NOT NULL,
	[PhoneNo] INT NOT NULL,
	[FirstName] NVARCHAR(100) NOT NULL,
	[MiddleName] NVARCHAR(100) NULL,
	[LastName] NVARCHAR(100) NOT NULL,
	[Initials] NVARCHAR(100) NULL,
	[CallingName] NVARCHAR(100) NULL,
	[NIC] NVARCHAR(20) NOT NULL,
	[DateOfBirth] DATETIME NOT NULL,

	CONSTRAINT [PK_StudentRegistration_StudentRegistrationId] PRIMARY KEY CLUSTERED ([StudentRegistrationId])
)
