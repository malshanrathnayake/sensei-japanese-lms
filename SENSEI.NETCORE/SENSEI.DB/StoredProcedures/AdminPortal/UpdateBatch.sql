CREATE PROCEDURE [dbo].[UpdateBatch]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY

		DECLARE @batchId BIGINT;

		SELECT @batchId = [BatchId]
		FROM OPENJSON(@jsonString, '$')
		WITH (
			[BatchId] BIGINT
		);

		IF(@batchId = 0)
		BEGIN

			INSERT INTO Batch ([CourseId], [BatchName],[BatchStartDate], [BatchEndDate])
			SELECT [CourseId], [BatchName], [BatchStartDate], [BatchEndDate]	
			FROM OPENJSON(@jsonString, '$')
			WITH (
				[CourseId] BIGINT,
				[BatchName] NVARCHAR(200),
				[BatchStartDate] DATETIME,
				[BatchEndDate] DATETIME
			);

			SET @primaryKey = SCOPE_IDENTITY();

		END
		ELSE
		BEGIN

			UPDATE Batch
			SET 
				[CourseId] = j.[CourseId],
				[BatchName] = j.[BatchName],
				[BatchStartDate] = j.[BatchStartDate],
				[BatchEndDate] = j.[BatchEndDate]
			FROM Batch c
			INNER JOIN OPENJSON(@jsonString, '$')
			WITH (
				[BatchId] BIGINT,
				[CourseId] BIGINT,	
				[BatchName] NVARCHAR(200),
				[BatchStartDate] DATETIME,
				[BatchEndDate] DATETIME
			) AS j ON c.[BatchId] = j.[BatchId]
			WHERE c.[BatchId] = @batchId;

			SET @primaryKey = @batchId;

		END


		COMMIT TRANSACTION;
		SET @executionStatus = 1;

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION;
		SET @executionStatus = 0;
        THROW;

    END CATCH

END
