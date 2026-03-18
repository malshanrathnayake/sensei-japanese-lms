CREATE PROCEDURE [dbo].[UpdateStaffData]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRAN;

        DECLARE @UserId BIGINT, @EmployeeId BIGINT;

        --malshan rathnayake
        IF NOT EXISTS (SELECT 1 FROM [dbo].[User] WHERE [UserName] = N'malshan.edu@gmail.com')
        BEGIN
            INSERT INTO [dbo].[User] ([UserName], [UserGlobalidentity], [CreatedDateTiime], [UserTypeEnum], [IsActive], [IsSuspend], [LastOtpSequence], [LastOtpSequencedateTime], [PhoneNo])
            VALUES( N'malshan.edu@gmail.com',NEWID(), GETDATE(), 2, 1, 0, NULL, NULL, N'+94711408116');
        
            SET @UserId = SCOPE_IDENTITY();

            INSERT INTO [dbo].[Staff] ( [UserId], [EmployeeCode],[Email],[FirstName],[MiddleName],[LastName], [Initials], [CallingName], [NIC], [IsDeleted], [PhoneNo])
            VALUES
            (@UserId, N'EMP-0001', N'malshan.edu@gmail.com', N'Malshan', N'', N'Rathnayake', N'W.K.G.P',N'Malshan',N'200009600581',0, N'+94711408116');

            SET @EmployeeId = SCOPE_IDENTITY();

            INSERT INTO [dbo].[EmployeeAddress]([EmployeeId], [AddressLineOne], [AddressLineTwo], [CountryId], [IsDeleted], [State], [City], [PostalCode])
            VALUES(@EmployeeId, N'No 124/B Paleepana', N'Pujapitiya', 1, 0, N'Central', N'Kandy', N'20112');
        END

        --santhushie nallaperuma
        IF NOT EXISTS (SELECT 1 FROM [dbo].[User] WHERE [UserName] = N'santhushie@gmail.com')
        BEGIN
            INSERT INTO [dbo].[User] ([UserName], [UserGlobalidentity], [CreatedDateTiime], [UserTypeEnum], [IsActive], [IsSuspend], [LastOtpSequence], [LastOtpSequencedateTime], [PhoneNo])
            VALUES( N'santhushie@gmail.com',NEWID(), GETDATE(), 2, 1, 0, NULL, NULL, N'+94762004123');

            SET @UserId = SCOPE_IDENTITY();

            INSERT INTO [dbo].[Staff] ( [UserId], [EmployeeCode],[Email],[FirstName],[MiddleName],[LastName], [Initials], [CallingName], [NIC], [IsDeleted], [PhoneNo])
            VALUES
            (@UserId, N'EMP-0001', N'santhushie@gmail.com', N'Santhushie', N'', N'Nallaperuma', N'S.N',N'Santhushie',N'200009600581',0, N'+94762004123');

            SET @EmployeeId = SCOPE_IDENTITY();

            INSERT INTO [dbo].[EmployeeAddress]([EmployeeId], [AddressLineOne], [AddressLineTwo], [CountryId], [IsDeleted], [State], [City], [PostalCode])
            VALUES(@EmployeeId, N'No 124/B Matara', N'Matara', 1, 0, N'Southern', N'Matara', N'80081');
        END

        --sensei japanese
        IF NOT EXISTS (SELECT 1 FROM [dbo].[User] WHERE [UserName] = N'senseijapanese07@gmail.com')
        BEGIN
            INSERT INTO [dbo].[User] ([UserName], [UserGlobalidentity], [CreatedDateTiime], [UserTypeEnum], [IsActive], [IsSuspend], [LastOtpSequence], [LastOtpSequencedateTime], [PhoneNo])
            VALUES( N'senseijapanese07@gmail.com',NEWID(), GETDATE(), 2, 1, 0, NULL, NULL, N'+94773833011');

            SET @UserId = SCOPE_IDENTITY();

            INSERT INTO [dbo].[Staff] ( [UserId], [EmployeeCode],[Email],[FirstName],[MiddleName],[LastName], [Initials], [CallingName], [NIC], [IsDeleted], [PhoneNo])
            VALUES
            (@UserId, N'EMP-0001', N'senseijapanese07@gmail.com', N'Sensei', N'', N'Japanese', N'S.J',N'Sensei',N'200403010706',0, N'+94773833011');

            SET @EmployeeId = SCOPE_IDENTITY();

            INSERT INTO [dbo].[EmployeeAddress]([EmployeeId], [AddressLineOne], [AddressLineTwo], [CountryId], [IsDeleted], [State], [City], [PostalCode])
            VALUES(@EmployeeId, N'No 13/1,Medagoda, Matara', N'Matara', 1, 0, N'Southern', N'Matara', N'80081');
        END

        COMMIT TRAN;

    END TRY
    BEGIN CATCH
        
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        THROW;

    END CATCH
END