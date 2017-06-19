WITH ClientRole_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			 (N'2885FDDD-9F03-48EB-8762-9BD176EBB496',	N'927fe4a9-4e27-4635-a208-eb5afc953294', N'f000aeed-5158-451d-b704-b1503f6af796')
			,(N'26C092FC-64DB-4334-87B5-09428F025C9C',	N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'f000aeed-5158-461d-b704-b1503f6af797')
		) AS ClientRole([ClientId], [RoleId], [Id])
	)
-- Reference Data for Role 
MERGE INTO dbo.[ClientRole] AS [Target]
USING ClientRole_CTE AS [Source]
	ON [Target].[ClientId] = [Source].[ClientId] and [Target].[RoleId] = [Source].[RoleId] and [Target].[Id] = [Source].[Id]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			[ClientId],[RoleId],[Id]
			)
		VALUES (
			 [Source].[ClientId]
			,[Source].[RoleId]
			,[Source].[Id]
			)
		OUTPUT $action, inserted.*, deleted.*;
