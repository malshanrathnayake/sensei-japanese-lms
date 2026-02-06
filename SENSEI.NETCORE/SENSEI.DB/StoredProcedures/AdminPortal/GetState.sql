CREATE PROCEDURE [dbo].[GetState]
	@countryId int = 0
AS
BEGIN

	SET NOCOUNT ON;

	IF(@countryId = 0)
	BEGIN
		SELECT * FROM State FOR JSON PATH;
	END
	ELSE
	BEGIN
		SELECT * FROM State WHERE CountryId = @countryId FOR JSON PATH;
	END

END
