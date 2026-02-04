CREATE TABLE [dbo].[EmployeeAddress]
(
    [EmployeeAddressId] BIGINT NOT NULL IDENTITY,
    [EmployeeId] BIGINT NOT NULL,
    [AddressLineOne] NVARCHAR(200) NOT NULL,
    [AddressLineTwo] NVARCHAR(200) NOT NULL,
    [CityId] INT NOT NULL,
    [IsDeleted] BIT NOT NULL,

    CONSTRAINT [PK_EmployeeAddress] PRIMARY KEY CLUSTERED ([EmployeeAddressId]),
    CONSTRAINT [FK_EmployeeAddress_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Staff]([EmployeeId]),
    CONSTRAINT [FK_EmployeeAddress_CityId] FOREIGN KEY ([CityId]) REFERENCES [City]([CityId])
);