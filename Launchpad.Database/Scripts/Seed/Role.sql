WITH Role_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			 (N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'Administrator')
			,(N'927fe4a9-4e27-4635-a208-eb5afc953294', N'Basic')			 
		) AS RoleSeed(Id, RoleName)
	)
-- Reference Data for Role 
MERGE INTO dbo.[ROLE] AS Target
USING Role_CTE AS Source
	ON Target.NAME = Source.RoleName
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			Id
			,NAME
			)
		VALUES (
			 Source.Id
			,Source.RoleName
			)
		OUTPUT $action, inserted.*, deleted.*;
