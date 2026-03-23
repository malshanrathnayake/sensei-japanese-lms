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
        DECLARE @email NVARCHAR(256), @phoneNo NVARCHAR(50);
		
		SELECT @studentRegistrationId = [StudentRegistrationId], @indexNumber = [IndexNumber], @batchId = [BatchId], @approvedById = [ApprovedById]
		FROM OPENJSON(@jsonString, '$')
		WITH
		(
			[StudentRegistrationId] BIGINT,
			[IndexNumber] NVARCHAR(100),
			[BatchId] BIGINT,
			[ApprovedById] BIGINT
		);

        SELECT @email = Email, @phoneNo = PhoneNo FROM StudentRegistration WHERE StudentRegistrationId = @studentRegistrationId;

		UPDATE [StudentRegistration]
		SET [IsApproved] = 1, [ApprovedById] = @approvedById, [UpdatedDateTime] = GETUTCDATE()
		WHERE [StudentRegistrationId] = @studentRegistrationId;

        -- Check if user exists
        SELECT @userId = UserId FROM [User] WHERE UserName = @email;

        IF (@userId IS NULL)
        BEGIN
		    INSERT INTO [User]([UserName], [UserGlobalidentity], [CreatedDateTiime], [UserTypeEnum], [IsActive], PhoneNo)
		    SELECT @email, NEWID(), GETUTCDATE(), 1, 1, @phoneNo;
		    SET @userId = SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE [User] SET IsActive = 1, PhoneNo = @phoneNo WHERE UserId = @userId;
        END

		INSERT INTO [Student]([UserId], [IndexNumber], [Email], [PhoneNo], [FirstName], [MiddleName], [LastName], [Initials], [CallingName], [NIC], [DateOfBirth], [BranchId], [StudentLearningModeId], [CountryId])
		SELECT @userId, @indexNumber, [Email], [PhoneNo], [FirstName], [MiddleName], [LastName], [Initials], [CallingName], [NIC], [DateOfBirth], [BranchId], [StudentLearningModeId], [CountryId]
		FROM [StudentRegistration]
		WHERE [StudentRegistrationId] = @studentRegistrationId;

		SET @primaryKey = SCOPE_IDENTITY();

		INSERT INTO [StudentAddress]([StudentId], [AddressLineOne], [AddressLineTwo], [CountryId], [State], [PostalCode], [City])
		SELECT @primaryKey, [AddressLineOne], [AddressLineTwo], [CountryId], [State], [PostalCode], [City]
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
