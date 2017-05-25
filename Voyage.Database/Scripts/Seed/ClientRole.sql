WITH ClientRole_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			 (N'2885FDDD-9F03-48EB-8762-9BD176EBB496',	N'927fe4a9-4e27-4635-a208-eb5afc953294')
			,(N'26C092FC-64DB-4334-87B5-09428F025C9C',	N'1cd39193-1f83-4d44-ab45-68c85be2acc8')
		) AS ClientRole([ClientId], [RoleId])
	)
-- Reference Data for Role 
MERGE INTO dbo.[ClientRole] AS [Target]
USING ClientRole_CTE AS [Source]
	ON [Target].[ClientId] = [Source].[ClientId] and [Target].[RoleId] = [Source].[RoleId]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			[ClientId],[RoleId]
			)
		VALUES (
			 [Source].[ClientId]
			,[Source].[RoleId]
			)
		OUTPUT $action, inserted.*, deleted.*;
