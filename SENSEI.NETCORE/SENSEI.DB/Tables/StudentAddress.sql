CREATE TABLE [dbo].[StudentAddress]
(
    [StudentAddressId] BIGINT NOT NULL IDENTITY,
    [StudentId] BIGINT NOT NULL,
    [AddressLineOne] NVARCHAR(200) NOT NULL,
    [AddressLineTwo] NVARCHAR(200) NOT NULL,
    [CityId] INT NOT NULL,
    [IsDeleted] BIT NOT NULL,

    CONSTRAINT [PK_StudentAddress] PRIMARY KEY CLUSTERED ([StudentAddressId]),
    CONSTRAINT [FK_StudentAddress_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Student]([StudentId]),
    CONSTRAINT [FK_StudentAddress_CityId] FOREIGN KEY ([CityId]) REFERENCES [City]([CityId])
);