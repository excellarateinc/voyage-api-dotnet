WITH RoleClaim_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			  (N'927fe4a9-4e27-4635-a208-eb5afc953294', N'authorities', N'api.users.roles.get')
			 ,(N'927fe4a9-4e27-4635-a208-eb5afc953294', N'authorities', N'api.users.roles.list')
			 ,(N'927fe4a9-4e27-4635-a208-eb5afc953294', N'authorities', N'api.roles.permission.get')
			 ,(N'927fe4a9-4e27-4635-a208-eb5afc953294', N'authorities', N'api.roles.permission.list')
			 ,(N'927fe4a9-4e27-4635-a208-eb5afc953294', N'authorities', N'api.users.get')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.roles.delete')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.roles.get')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.roles.update')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.roles.permission.add')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.roles.permission.delete')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.roles.permission.list')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.roles.permission.get')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.permission.create')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.permission.delete')			 			 
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.permission.get')			 
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.permission.list')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.permissions.list')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.roles.assign')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.roles.create')	
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.roles.delete')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.roles.get')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.roles.list')			 			 
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.list')			 
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.get')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.update')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.delete')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.users.create')
			 ,(N'927fe4a9-4e27-4635-a208-eb5afc953294', N'authorities', N'api.notifications.list')	
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'authorities', N'api.notifications.list')		 
		) AS RoleClaimSeed([RoleId], [ClaimType], [ClaimValue])
	)

-- Reference Data for Role Claim
MERGE INTO dbo.[ROLECLAIM] AS [Target]
USING RoleClaim_CTE AS [Source]
	ON [Target].[RoleId] = [Source].[RoleId] and [Target].[ClaimValue] = [Source].[ClaimValue]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			  [RoleId]
			, [ClaimType]
			, [ClaimValue]
			)
		VALUES (
			
			  [Source].[RoleId]
			, [Source].[ClaimType]
			, [Source].[ClaimValue]
			)
WHEN MATCHED 
	THEN
		UPDATE SET [Target].[ClaimType] = [Source].[ClaimType]
	OUTPUT $action, inserted.*, deleted.*;
