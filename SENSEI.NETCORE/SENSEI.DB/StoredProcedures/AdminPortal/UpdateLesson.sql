CREATE PROCEDURE [dbo].[UpdateLesson]
	@jsonString NVARCHAR(MAX) = '',
	@executionStatus BIT OUT,
	@primaryKey BIGINT OUT
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRANSACTION;

	BEGIN TRY

		DECLARE @lessonId INT;

		SELECT @lessonId = [LessonId]
		FROM OPENJSON(@jsonString, '$')
		WITH (
			[LessonId] BIGINT
		);

		IF(@lessonId = 0)
		BEGIN

			INSERT INTO Lesson ([CourseId], [LessonName], [Description])
			SELECT [CourseId], [LessonName], [Description]	
			FROM OPENJSON(@jsonString, '$')
			WITH (
				[CourseId] BIGINT,
				[LessonName] NVARCHAR(200),
				[Description] NVARCHAR(MAX)
			);

			SET @primaryKey = SCOPE_IDENTITY();

		END
		ELSE
		BEGIN

			UPDATE Lesson
			SET 
				[CourseId] = j.[CourseId],
				[LessonName] = j.[LessonName],
				[Description] = j.[Description]
			FROM Lesson c
			INNER JOIN OPENJSON(@jsonString, '$')
			WITH (
				[LessonId] INT,
				[CourseId] BIGINT,	
				[LessonName] NVARCHAR(200),
				[Description] NVARCHAR(MAX)
			) AS j ON c.[LessonId] = j.[LessonId]
			WHERE c.[LessonId] = @lessonId;

			SET @primaryKey = @lessonId;

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
