CREATE PROCEDURE [dbo].[UpdateStudent]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY
		DECLARE @studentId BIGINT;

		SELECT @studentId = [StudentId]
		FROM OPENJSON(@jsonString, '$')
		WITH (
			[StudentId] BIGINT
		);

		IF(@studentId > 0)
		BEGIN
			UPDATE Student
			SET 
				[IndexNumber] = j.[IndexNumber],
				[Email] = j.[Email],
				[PhoneNo] = j.[PhoneNo],
				[FirstName] = j.[FirstName],
				[MiddleName] = j.[MiddleName],
				[LastName] = j.[LastName],
				[Initials] = j.[Initials],
				[CallingName] = j.[CallingName],
				[NIC] = j.[NIC],
				[DateOfBirth] = j.[DateOfBirth],
				[BranchId] = j.[BranchId],
				[StudentLearningModeId] = j.[StudentLearningModeId],
				[CountryId] = j.[CountryId]
			FROM Student s
			INNER JOIN OPENJSON(@jsonString, '$')
			WITH (
				[StudentId] BIGINT,
				[IndexNumber] NVARCHAR(100),
				[Email] NVARCHAR(200),
				[PhoneNo] NVARCHAR(100),
				[FirstName] NVARCHAR(200),
				[MiddleName] NVARCHAR(200),
				[LastName] NVARCHAR(200),
				[Initials] NVARCHAR(200),
				[CallingName] NVARCHAR(200),
				[NIC] NVARCHAR(200),
				[DateOfBirth] DATETIME,
				[BranchId] INT,
				[StudentLearningModeId] INT,
				[CountryId] INT
			) AS j ON s.[StudentId] = j.[StudentId]
			WHERE s.[StudentId] = @studentId;

			-- Update the associated User email as well
			UPDATE [User]
			SET UserName = (SELECT Email FROM Student WHERE StudentId = @studentId)
			WHERE UserId = (SELECT UserId FROM Student WHERE StudentId = @studentId);

			SET @primaryKey = @studentId;
		END

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		SET @primaryKey = 0;
		THROW;
	END CATCH
END
