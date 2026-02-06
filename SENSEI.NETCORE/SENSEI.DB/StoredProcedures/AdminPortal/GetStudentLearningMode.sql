CREATE PROCEDURE [dbo].[GetStudentLearningMode]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT * FROM StudentLearningMode FOR JSON PATH;

END