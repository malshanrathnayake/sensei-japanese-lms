CREATE TABLE [dbo].[StudentRegistration]
(
	[StudentRegistrationId] BIGINT NOT NULL IDENTITY,
	[Email] NVARCHAR(256) NOT NULL,
	[PhoneNo] NVARCHAR(100) NULL,
	[FirstName] NVARCHAR(100) NOT NULL,
	[MiddleName] NVARCHAR(100) NULL,
	[LastName] NVARCHAR(100) NOT NULL,
	[Initials] NVARCHAR(100) NULL,
	[CallingName] NVARCHAR(100) NULL,
	[NIC] NVARCHAR(20) NOT NULL,
	[DateOfBirth] DATETIME NOT NULL,
	[IsApproved] BIT NOT NULL DEFAULT 0,
	[ApprovedById] BIGINT NULL,
	[AddressLineOne] NVARCHAR(200) NOT NULL,
	[AddressLineTwo] NVARCHAR(200) NULL,
	[City] NVARCHAR(20) NULL,
	[PostalCode] NVARCHAR(20) NOT NULL,
	[BranchId] BIGINT NULL,
	[StudentLearningModeId] INT NOT NULL,
	[CourseId] BIGINT NOT NULL,
	[CreatedDateTime] DATETIME NULL,
	[CountryId] INT NULL,
	[UpdatedDateTime] DATETIME NULL,
	[State] NVARCHAR(100) NULL

	CONSTRAINT [PK_StudentRegistration_StudentRegistrationId] PRIMARY KEY CLUSTERED ([StudentRegistrationId])
)
