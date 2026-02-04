CREATE TABLE [dbo].[Country]
(
	[CountryId] INT NOT NULL IDENTITY,
	[CountryName] NVARCHAR(100) NOT NULL,

	CONSTRAINT [PK_Country_CountryId] PRIMARY KEY CLUSTERED ([CountryId])
)
