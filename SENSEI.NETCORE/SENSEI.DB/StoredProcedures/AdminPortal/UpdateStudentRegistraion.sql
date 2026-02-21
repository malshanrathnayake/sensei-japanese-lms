CREATE PROCEDURE [dbo].[UpdateStudentRegistraion]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY

		INSERT INTO [dbo].[StudentRegistration]
		(
			[Email],
			[PhoneNo],
			[FirstName],
			[MiddleName],
			[LastName],
			[Initials],
			[CallingName],
			[NIC],
			[DateOfBirth],
			[ApprovedById],
			[AddressLineOne],
			[AddressLineTwo],
			[CityId],
			[PostalCode],
			[BranchId],
			[StudentLearningModeId],
			[CourseId],
			[CreatedDateTime],
			[CountryId]
		)
		SELECT
			[Email],
			[PhoneNo],
			[FirstName],
			[MiddleName],
			[LastName],
			[Initials],
			[CallingName],
			[NIC],
			[DateOfBirth],
			[ApprovedById],
			[AddressLineOne],
			[AddressLineTwo],
			[CityId],
			[PostalCode],
			[BranchId],
			[StudentLearningModeId],
			[CourseId],
			GETUTCDATE(),
			[CountryId]
		FROM OPENJSON(@jsonString, '$')
		WITH
		(
			[Email] NVARCHAR(256),
			[PhoneNo] INT,
			[FirstName] NVARCHAR(100),
			[MiddleName] NVARCHAR(100),
			[LastName] NVARCHAR(100),
			[Initials] NVARCHAR(100),
			[CallingName] NVARCHAR(100),
			[NIC] NVARCHAR(20),
			[DateOfBirth] DATETIME,
			[ApprovedById] BIGINT,
			[AddressLineOne] NVARCHAR(200),
			[AddressLineTwo] NVARCHAR(200),
			[CityId] BIGINT,
			[PostalCode] NVARCHAR(20),
			[BranchId] BIGINT,
			[StudentLearningModeId] INT,
			[CourseId] BIGINT,
			[CountryId] INT
		);

		SET @primaryKey = SCOPE_IDENTITY();

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		THROW;

	END CATCH

END
