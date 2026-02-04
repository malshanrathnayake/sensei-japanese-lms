CREATE TABLE [dbo].[State]
(
	[StateId] INT NOT NULL IDENTITY,
	[CountryId] INT NOT NULL,
	[StateName] NVARCHAR(100) NOT NULL,

	CONSTRAINT [PK_State_StateId] PRIMARY KEY CLUSTERED ([StateId]),
	CONSTRAINT [FK_State_Country_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country]([CountryId])
)
