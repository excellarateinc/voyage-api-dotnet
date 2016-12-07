WITH RoleClaim_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			  (N'927fe4a9-4e27-4635-a208-eb5afc953294', N'lss.permission', N'login')
			 ,(N'927fe4a9-4e27-4635-a208-eb5afc953294', N'lss.permission', N'list.user-claims')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'assign.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'create.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'delete.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'list.roles')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'revoke.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'view.role')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'view.claim')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'list.users')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'list.user-claims')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'view.user')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'update.user')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'delete.user')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'create.user')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'list.widgets')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'view.widget')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'update.widget')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'create.widget')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'delete.widget')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'list.role-claims')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'delete.role-claim')
			 ,(N'1cd39193-1f83-4d44-ab45-68c85be2acc8', N'lss.permission', N'create.claim')	 
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
