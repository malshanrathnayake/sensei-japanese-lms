CREATE PROCEDURE [dbo].[ApproveStudentRegistraion]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY


		DECLARE @studentRegistrationId BIGINT, @indexNumber NVARCHAR(100), @batchId BIGINT, @approvedById BIGINT;
		DECLARE @userId BIGINT;
		
		SELECT @studentRegistrationId = [StudentRegistrationId], @indexNumber = [IndexNumber], @batchId = [BatchId], @approvedById = [ApprovedById]
		FROM OPENJSON(@jsonString, '$')
		WITH
		(
			[StudentRegistrationId] BIGINT,
			[IndexNumber] NVARCHAR(100),
			[BatchId] BIGINT,
			[ApprovedById] BIGINT
		);

		UPDATE [StudentRegistration]
		SET [IsApproved] = 1, [ApprovedById] = @approvedById, [UpdatedDateTime] = GETUTCDATE()
		WHERE [StudentRegistrationId] = @studentRegistrationId;

		INSERT INTO [User]([UserName], [UserGlobalidentity], [CreatedDateTiime], [UserTypeEnum], [IsActive])
		SELECT Email, NEWID(), GETUTCDATE(), 1, 1
		FROM [StudentRegistration]
		WHERE [StudentRegistrationId] = @studentRegistrationId;

		SET @userId = SCOPE_IDENTITY();

		INSERT INTO [Student]([UserId], [IndexNumber], [Email], [PhoneNo], [FirstName], [MiddleName], [LastName], [Initials], [CallingName], [NIC], [DateOfBirth], [BranchId], [StudentLearningModeId], [CountryId])
		SELECT @userId, @indexNumber, [Email], [PhoneNo], [FirstName], [MiddleName], [LastName], [Initials], [CallingName], [NIC], [DateOfBirth], [BranchId], [StudentLearningModeId], [CountryId]
		FROM [StudentRegistration]
		WHERE [StudentRegistrationId] = @studentRegistrationId;

		SET @primaryKey = SCOPE_IDENTITY();

		INSERT INTO [StudentAddress]([StudentId], [AddressLineOne], [AddressLineTwo], [CountryId], [State], [PostalCode])
		SELECT @primaryKey, [AddressLineOne], [AddressLineTwo], [CountryId], [State], [PostalCode]
		FROM [StudentRegistration]
		WHERE [StudentRegistrationId] = @studentRegistrationId;

		INSERT INTO StudentBatch(BatchId, StudentId)
		VALUES(@batchId, @primaryKey);

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		THROW;

	END CATCH

END
