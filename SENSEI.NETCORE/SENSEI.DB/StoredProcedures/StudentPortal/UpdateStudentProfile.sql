CREATE PROCEDURE [dbo].[UpdateStudentProfile]
	@studentJson NVARCHAR(MAX) = ''
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @studentId BIGINT;

	SELECT @studentId = StudentId FROM OPENJSON(@studentJson) WITH (StudentId BIGINT);

	IF EXISTS (SELECT 1 FROM Student WHERE StudentId = @studentId)
	BEGIN
		UPDATE Student
		SET 
			FirstName = J.FirstName,
			LastName = J.LastName,
			PhoneNo = J.PhoneNo,
			NIC = J.NIC,
			UpdatedAt = GETDATE()
		FROM OPENJSON(@studentJson)
		WITH (
			FirstName NVARCHAR(100),
			LastName NVARCHAR(100),
			PhoneNo NVARCHAR(20),
			NIC NVARCHAR(20)
		) J
		WHERE Student.StudentId = @studentId;

		SELECT CAST(1 AS BIT) AS 'Success', @studentId AS 'PrimaryKey';
	END
	ELSE
	BEGIN
		SELECT CAST(0 AS BIT) AS 'Success', CAST(0 AS BIGINT) AS 'PrimaryKey';
	END

END
