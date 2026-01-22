CREATE TABLE [dbo].[EmployeeAddress]
(
    [EmployeeAddressId] BIGINT NOT NULL IDENTITY,
    [EmployeeId] BIGINT NOT NULL,
    [AddressLineOne] NVARCHAR(200) NOT NULL,
    [AddressLineTwo] NVARCHAR(200) NOT NULL,
    [City] NVARCHAR(200) NOT NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_EmployeeAddress] PRIMARY KEY CLUSTERED ([EmployeeAddressId])
);