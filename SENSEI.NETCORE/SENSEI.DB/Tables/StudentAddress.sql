CREATE TABLE [dbo].[StudentAddress]
(
    [StudentAddressId] BIGINT NOT NULL IDENTITY,
    [StudentId] BIGINT NOT NULL,
    [AddressLineOne] NVARCHAR(200) NOT NULL,
    [AddressLineTwo] NVARCHAR(200) NOT NULL,
    [City] NVARCHAR(200) NOT NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_StudentAddress] PRIMARY KEY CLUSTERED ([StudentAddressId])
);