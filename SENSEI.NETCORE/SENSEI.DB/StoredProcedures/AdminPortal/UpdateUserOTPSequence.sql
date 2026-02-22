CREATE PROCEDURE [dbo].[UpdateUserOTPSequence]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY

		DECLARE @userId BIGINT;

		SELECT @userId = [UserId]
		FROM OPENJSON(@jsonString, '$')
		WITH (
			[UserId] BIGINT
		);

		UPDATE [User]
		SET 
			[LastOtpSequence] = j.[LastOtpSequence],
			[LastOtpSequencedateTime] = GETUTCDATE()
		FROM [User] u
		INNER JOIN OPENJSON(@jsonString, '$')
		WITH (
			[UserId] BIGINT,
			[LastOtpSequence] INT
		) AS j ON u.[UserId] = j.[UserId]
		WHERE u.[UserId] = @userId;

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
        THROW;

    END CATCH

END