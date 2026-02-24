sELECT * FROM [StudentRegistration]

SELECT * FROM Country


	SELECT STREG.* 
	FROM StudentRegistration STREG
	INNER JOIN Course C ON C.CourseId = STREG.CourseId
	INNER JOIN StudentLearningMode SLM ON SLM.StudentLearningModeId = STREG.StudentLearningModeId
	WHERE STREG.IsApproved = 0
	FOR JSON PATH;

    SELECT * FROM Student
    SELECT * FROM [User]
	SELECT * FROM [Staff]
	SELECT * FROM [StudentRegistration]

	

	DECLARE @UserId BIGINT, @EmployeeId BIGINT;

INSERT INTO [dbo].[User] ([UserName], [UserGlobalidentity], [CreatedDateTiime], [UserTypeEnum], [IsActive], [IsSuspend], [LastOtpSequence], [LastOtpSequencedateTime], [PhoneNo])
VALUES( N'malshan.edu@gmail.com',NEWID(), GETDATE(), 2, 1, 0, NULL, NULL, N'+94711408116');

SET @UserId = SCOPE_IDENTITY();

INSERT INTO [dbo].[Staff] ( [UserId], [EmployeeCode],[Email],[FirstName],[MiddleName],[LastName], [Initials], [CallingName], [NIC], [IsDeleted], [PhoneNo])
VALUES
(@UserId, N'EMP-0001', N'malshan.edu@gmail.com', N'Malshan', N'', N'Rathnayake', N'W.K.G.P',N'Malshan',N'200009600581',0, N'+94711408116');

SET @EmployeeId = SCOPE_IDENTITY();

INSERT INTO [dbo].[EmployeeAddress]([EmployeeId], [AddressLineOne], [AddressLineTwo], [CountryId], [IsDeleted], [State], [City], [PostalCode])
VALUES(@EmployeeId, N'No 124/B Paleepana', N'Pujapitiya', 1, 0, N'Central', N'Kandy', N'20112');

    SELECT * FROM [User]
	    SELECT * FROM [Staff]
		    SELECT * FROM [EmployeeAddress]

			SELECT GETUTCDATE()