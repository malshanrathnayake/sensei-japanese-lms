CREATE PROCEDURE [dbo].[UpdateCourse]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	BEGIN TRANSACTION;

	BEGIN TRY

		DECLARE @courseId INT;

		SELECT @courseId = [CourseId]
		FROM OPENJSON(@jsonString, '$')
		WITH (
			[CourseId] BIGINT
		);

		IF(@courseId = 0)
		BEGIN

			INSERT INTO Course ([CourseName], [CourseCode])
			SELECT [CourseName], [CourseCode]
			FROM OPENJSON(@jsonString, '$')
			WITH (
				[CourseName] NVARCHAR(200),
				[CourseCode] NVARCHAR(200)
			);

			SET @primaryKey = SCOPE_IDENTITY();

		END
		ELSE
		BEGIN

			UPDATE Course
			SET 
				[CourseName] = j.[CourseName],
				[CourseCode] = j.[CourseCode]
			FROM Course c
			INNER JOIN OPENJSON(@jsonString, '$')
			WITH (
				[CourseId] INT,
				[CourseName] NVARCHAR(200),
				[CourseCode] NVARCHAR(200)
			) AS j ON c.[CourseId] = j.[CourseId]
			WHERE c.[CourseId] = @courseId;

			SET @primaryKey = @courseId;

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
