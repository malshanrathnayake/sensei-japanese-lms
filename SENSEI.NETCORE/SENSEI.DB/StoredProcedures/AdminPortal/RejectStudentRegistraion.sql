CREATE PROCEDURE [dbo].[RejectStudentRegistraion]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY


		DECLARE @studentRegistrationId BIGINT, @rejectComment NVARCHAR(100), @approvedById BIGINT;
		DECLARE @userId BIGINT;
		
		SELECT @studentRegistrationId = [StudentRegistrationId], @rejectComment = [RejectionComment], @approvedById = [ApprovedById]
		FROM OPENJSON(@jsonString, '$')
		WITH
		(
			[StudentRegistrationId] BIGINT,
			[RejectionComment] NVARCHAR(100),
			[ApprovedById] BIGINT
		);

		UPDATE [StudentRegistration]
		SET [IsApproved] = 0, [IsRejected] = 1, [ApprovedById] = @approvedById, [UpdatedDateTime] = GETUTCDATE(), [RejectionComment] = @rejectComment
		WHERE [StudentRegistrationId] = @studentRegistrationId;

		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
		THROW;

	END CATCH

END