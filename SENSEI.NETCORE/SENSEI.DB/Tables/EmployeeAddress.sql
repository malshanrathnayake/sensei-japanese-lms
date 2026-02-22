CREATE TABLE [dbo].[EmployeeAddress]
(
    [EmployeeAddressId] BIGINT NOT NULL IDENTITY,
    [EmployeeId] BIGINT NOT NULL,
    [AddressLineOne] NVARCHAR(200) NOT NULL,
    [AddressLineTwo] NVARCHAR(200) NOT NULL,
    [CountryId] INT NOT NULL,
    [IsDeleted] BIT NOT NULL,
    [State] NVARCHAR(100) NULL,
    [City] NVARCHAR(100) NULL,
    [PostalCode] NVARCHAR(20) NULL,

    CONSTRAINT [PK_EmployeeAddress] PRIMARY KEY CLUSTERED ([EmployeeAddressId]),
    CONSTRAINT [FK_EmployeeAddress_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Staff]([EmployeeId]),
    CONSTRAINT [FK_EmployeeAddress_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Country]([CountryId])
);