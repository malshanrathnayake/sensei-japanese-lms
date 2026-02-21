CREATE TABLE [dbo].[StudentAddress]
(
    [StudentAddressId] BIGINT NOT NULL IDENTITY,
    [StudentId] BIGINT NOT NULL,
    [AddressLineOne] NVARCHAR(200) NOT NULL,
    [AddressLineTwo] NVARCHAR(200) NOT NULL,
    [CountryId] INT NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [State] NVARCHAR(100) NULL,
    [City] NVARCHAR(100) NULL,
    [PostalCode] NVARCHAR(20) NULL,

    CONSTRAINT [PK_StudentAddress] PRIMARY KEY CLUSTERED ([StudentAddressId]),
    CONSTRAINT [FK_StudentAddress_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Student]([StudentId]),
    CONSTRAINT [FK_StudentAddress_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Country]([CountryId])
);