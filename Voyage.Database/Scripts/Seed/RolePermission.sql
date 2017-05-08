WITH RolePermission_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			  (N'927fe4a9-4e27-4635-a208-eb5afc953294', N'app.permission', N'login')
			 ,(N'927fe4a9-4e27-4635-a208-eb5afc953294', N'app.permission', N'list.user-claims')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'assign.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'create.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'delete.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'list.roles')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'revoke.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'view.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'view.claim')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'list.users')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'list.user-claims')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'view.user')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'update.user')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'delete.user')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'create.user')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'list.role-claims')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'delete.role-claim')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'app.permission', N'create.claim')	 
		) AS RolePermissionSeed([RoleId], [PermissionType], [PermissionValue])
	)

-- Reference Data for Role Claim
MERGE INTO dbo.[ROLEPERMISSION] AS [Target]
USING RolePermission_CTE AS [Source]
	ON [Target].[RoleId] = [Source].[RoleId] and [Target].[PermissionValue] = [Source].[PermissionValue]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			  [RoleId]
			, [PermissionType]
			, [PermissionValue]
			)
		VALUES (
			
			  [Source].[RoleId]
			, [Source].[PermissionType]
			, [Source].[PermissionValue]
			)
WHEN MATCHED 
	THEN
		UPDATE SET [Target].[PermissionType] = [Source].[PermissionType]
	OUTPUT $action, inserted.*, deleted.*;
