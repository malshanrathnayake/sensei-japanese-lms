CREATE PROCEDURE [dbo].[GetUser]
	@userId BIGINT = 0,
	@phoneNo NVARCHAR(255) = '',
	@email NVARCHAR(255) = '',
	@userGlobalIdentity NVARCHAR(255) = ''
AS
BEGIN

	IF @userId <> 0
	BEGIN
		SELECT U.*,
			JSON_QUERY(ISNULL((SELECT Student.* FROM Student WHERE Student.UserId = U.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Student',
			JSON_QUERY(ISNULL((SELECT Staff.* FROM Staff WHERE Staff.UserId = U.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Staff'
		FROM [User] U WHERE U.UserId = @userId FOR JSON PATH
	END
	ELSE IF (RTRIM(ISNULL(@phoneNo, ''))) <> ''
	BEGIN
		SELECT U.*,
			JSON_QUERY(ISNULL((SELECT Student.* FROM Student WHERE Student.UserId = U.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Student',
			JSON_QUERY(ISNULL((SELECT Staff.* FROM Staff WHERE Staff.UserId = U.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Staff'
		FROM [User] U WHERE U.PhoneNo = @phoneNo FOR JSON PATH
	END
	ELSE IF LTRIM(RTRIM(ISNULL(@email, ''))) <> ''
    BEGIN
        SELECT U.*,
			JSON_QUERY(ISNULL((SELECT Student.* FROM Student WHERE Student.UserId = U.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Student',
			JSON_QUERY(ISNULL((SELECT Staff.* FROM Staff WHERE Staff.UserId = U.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Staff'
		FROM [User] U WHERE U.UserName = @email FOR JSON PATH
    END
	ELSE IF LTRIM(RTRIM(ISNULL(@userGlobalIdentity, ''))) <> ''
	BEGIN
		SELECT U.*,
			JSON_QUERY(ISNULL((SELECT Student.* FROM Student WHERE Student.UserId = U.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Student',
			JSON_QUERY(ISNULL((SELECT Staff.* FROM Staff WHERE Staff.UserId = U.UserId FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), null)) AS 'Staff'
		FROM [User] U WHERE U.UserGlobalIdentity = @userGlobalIdentity FOR JSON PATH
	END

END