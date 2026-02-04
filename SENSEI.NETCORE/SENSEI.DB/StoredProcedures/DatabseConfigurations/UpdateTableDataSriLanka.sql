CREATE PROCEDURE [dbo].[UpdateTableDataSriLanka]
AS
BEGIN

	SET NOCOUNT ON;

    --Country
	INSERT INTO Country (CountryName)
    VALUES ('Sri Lanka');

	DECLARE @SriLankaId INT;
	SELECT @SriLankaId = CountryId FROM Country WHERE CountryName = 'Sri Lanka';

	--state
	INSERT INTO State (CountryId, StateName) VALUES (@SriLankaId, 'Western Province');
	INSERT INTO State (CountryId, StateName) VALUES (@SriLankaId, 'Central Province');
	INSERT INTO State (CountryId, StateName) VALUES (@SriLankaId, 'Southern Province');
	INSERT INTO State (CountryId, StateName) VALUES (@SriLankaId, 'Eastern Province');
	INSERT INTO State (CountryId, StateName) VALUES (@SriLankaId, 'Northern Province');
	INSERT INTO State (CountryId, StateName) VALUES (@SriLankaId, 'North Western Province');
	INSERT INTO State (CountryId, StateName) VALUES (@SriLankaId, 'North Central Province');
	INSERT INTO State (CountryId, StateName) VALUES (@SriLankaId, 'Sabaragamuwa Province');
	INSERT INTO State (CountryId, StateName) VALUES (@SriLankaId, 'Uva Province');

	DECLARE @WesternProvinceId INT;
	SELECT @WesternProvinceId = StateId FROM State WHERE StateName = 'Western Province';

	DECLARE @CentralProvinceId INT;
	SELECT @CentralProvinceId = StateId FROM State WHERE StateName = 'Central Province';

	DECLARE @SouthernProvinceId INT;
	SELECT @SouthernProvinceId = StateId FROM State WHERE StateName = 'Southern Province';

	DECLARE @EasternProvinceId INT;
	SELECT @EasternProvinceId = StateId FROM State WHERE StateName = 'Eastern Province';

	DECLARE @NorthernProvinceId INT;
	SELECT @NorthernProvinceId = StateId FROM State WHERE StateName = 'Northern Province';

	DECLARE @NorthWesternProvinceId INT;
	SELECT @NorthWesternProvinceId = StateId FROM State WHERE StateName = 'North Western Province';

	DECLARE @NorthCentralProvinceId INT;
	SELECT @NorthCentralProvinceId = StateId FROM State WHERE StateName = 'North Central Province';

	DECLARE @SabaragamuwaProvinceId INT;
	SELECT @SabaragamuwaProvinceId = StateId FROM State WHERE StateName = 'Sabaragamuwa Province';

	DECLARE @UvaProvinceId INT;
	SELECT @UvaProvinceId = StateId FROM State WHERE StateName = 'Uva Province';


	--city
	INSERT INTO City (StateId, CityName) VALUES (@WesternProvinceId, 'Colombo');
	INSERT INTO City (StateId, CityName) VALUES (@WesternProvinceId, 'Gampaha');
	INSERT INTO City (StateId, CityName) VALUES (@WesternProvinceId, 'Kalutara');

	INSERT INTO City (StateId, CityName) VALUES (@CentralProvinceId, 'Kandy');
	INSERT INTO City (StateId, CityName) VALUES (@CentralProvinceId, 'Matale');
	INSERT INTO City (StateId, CityName) VALUES (@CentralProvinceId, 'Nuwara Eliya');

	INSERT INTO City (StateId, CityName) VALUES (@SouthernProvinceId, 'Galle');
	INSERT INTO City (StateId, CityName) VALUES (@SouthernProvinceId, 'Matara');
	INSERT INTO City (StateId, CityName) VALUES (@SouthernProvinceId, 'Hambantota');

	INSERT INTO City (StateId, CityName) VALUES (@EasternProvinceId, 'Ampara');
	INSERT INTO City (StateId, CityName) VALUES (@EasternProvinceId, 'Batticaloa');
	INSERT INTO City (StateId, CityName) VALUES (@EasternProvinceId, 'Trincomalee');

	INSERT INTO City (StateId, CityName) VALUES (@NorthernProvinceId, 'Jaffna');
	INSERT INTO City (StateId, CityName) VALUES (@NorthernProvinceId, 'Kilinochchi');
	INSERT INTO City (StateId, CityName) VALUES (@NorthernProvinceId, 'Mannar');
	INSERT INTO City (StateId, CityName) VALUES (@NorthernProvinceId, 'Mullaitivu');
	INSERT INTO City (StateId, CityName) VALUES (@NorthernProvinceId, 'Vavuniya');

	INSERT INTO City (StateId, CityName) VALUES (@NorthWesternProvinceId, 'Kurunegala');
	INSERT INTO City (StateId, CityName) VALUES (@NorthWesternProvinceId, 'Puttalam');

	INSERT INTO City (StateId, CityName) VALUES (@NorthCentralProvinceId, 'Anuradhapura');
	INSERT INTO City (StateId, CityName) VALUES (@NorthCentralProvinceId, 'Polonnaruwa');

	INSERT INTO City (StateId, CityName) VALUES (@SabaragamuwaProvinceId, 'Ratnapura');
	INSERT INTO City (StateId, CityName) VALUES (@SabaragamuwaProvinceId, 'Kegalle');

	INSERT INTO City (StateId, CityName) VALUES (@UvaProvinceId, 'Badulla');
	INSERT INTO City (StateId, CityName) VALUES (@UvaProvinceId, ' Monaragala');

	--learning mode
	INSERT INTO StudentLearningMode (LearningModeName) VALUES ('Online');
	INSERT INTO StudentLearningMode (LearningModeName) VALUES ('In Class');

END
