WITH RoleClaim_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			  (CONVERT(uniqueidentifier, '2fcac24c-98a5-42d3-815d-671371205ae5'),CONVERT(uniqueidentifier, '927fe4a9-4e27-4635-a208-eb5afc953294'), N'authorities', N'api.users.roles.get')
			 ,(CONVERT(uniqueidentifier, '7a50aa06-69df-4773-b018-6d6a02f81d62'),CONVERT(uniqueidentifier, '927fe4a9-4e27-4635-a208-eb5afc953294'), N'authorities', N'api.users.roles.list')
			 ,(CONVERT(uniqueidentifier, '41ff127f-cc5b-4026-8324-ea6566ccbd9a'),CONVERT(uniqueidentifier, '927fe4a9-4e27-4635-a208-eb5afc953294'), N'authorities', N'api.roles.permission.get')
			 ,(CONVERT(uniqueidentifier, 'b8a79514-2975-4a76-8aba-b6c12281f5c1'),CONVERT(uniqueidentifier, '927fe4a9-4e27-4635-a208-eb5afc953294'), N'authorities', N'api.roles.permission.list')
			 ,(CONVERT(uniqueidentifier, '1abf446c-543f-45b3-ad89-cc91df8e5907'),CONVERT(uniqueidentifier, '927fe4a9-4e27-4635-a208-eb5afc953294'), N'authorities', N'api.users.get')
			 ,(CONVERT(uniqueidentifier, 'a3ea6d55-2e62-4042-a7e6-3dfac98b4135'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.roles.delete')
			 ,(CONVERT(uniqueidentifier, 'a8822e82-22f7-47cc-b3fa-f5679ada7406'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.roles.get')
			 ,(CONVERT(uniqueidentifier, 'fd53d709-685f-4368-bdef-ded06b4add32'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.roles.update')
			 ,(CONVERT(uniqueidentifier, 'e8dbcb4e-da9e-427b-b091-6128dcc49387'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.roles.permission.add')
			 ,(CONVERT(uniqueidentifier, 'b7a53c9a-564a-413f-b975-c856548d3e91'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.roles.permission.delete')
			 ,(CONVERT(uniqueidentifier, '1d4190f3-1e46-459e-aabf-dfef001d6715'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.roles.permission.list')
			 ,(CONVERT(uniqueidentifier, '961d22b5-60d8-4923-8d63-00aa1f594e83'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.roles.permission.get')
			 ,(CONVERT(uniqueidentifier, '369476d0-99d7-4a90-8621-191ae8a06b53'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.permission.create')
			 ,(CONVERT(uniqueidentifier, '807b3ad7-98f4-453b-a89b-9509d01b809f'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.permission.delete')			 			 
			 ,(CONVERT(uniqueidentifier, 'e9837d98-8440-4bf1-806f-c06e0d18f9c2'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.permission.get')			 
			 ,(CONVERT(uniqueidentifier, '2ab9f75d-c794-450c-8713-16b75dca06d5'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.permission.list')
			 ,(CONVERT(uniqueidentifier, 'e5398db9-344f-4750-810b-13b7ed561583'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.permissions.list')
			 ,(CONVERT(uniqueidentifier, '346a15be-dbfa-4aa1-853a-e8bd6898e550'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.roles.assign')
			 ,(CONVERT(uniqueidentifier, '67d0a5e3-92f5-4ac8-b45a-bcc1426059d5'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.roles.create')	
			 ,(CONVERT(uniqueidentifier, 'c7124d9f-ab2c-42ff-a353-a7e887d34d9e'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.roles.delete')
			 ,(CONVERT(uniqueidentifier, 'b1b6df70-a202-439d-9b72-c036ca03ff07'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.roles.get')
			 ,(CONVERT(uniqueidentifier, '21f8465e-463d-40cc-9d83-b466b3c702a1'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.roles.list')			 			 
			 ,(CONVERT(uniqueidentifier, 'b4da6d4e-30e0-4854-991b-1174db06b7f8'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.list')			 
			 ,(CONVERT(uniqueidentifier, '20bb11eb-c295-4e35-8c58-ea8a4eb563b2'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.get')
			 ,(CONVERT(uniqueidentifier, 'eba11cc9-364f-446d-b0f2-4b5ab333346d'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.update')
			 ,(CONVERT(uniqueidentifier, 'febdee60-671f-459f-9bec-b979943a3dde'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.delete')
			 ,(CONVERT(uniqueidentifier, '1cc00aed-4a21-4de2-90e7-5145bc70d33b'),CONVERT(uniqueidentifier, '1cd39193-1f83-4d44-ab45-68c85be2acc8'), N'authorities', N'api.users.create')		 
		) AS RoleClaimSeed([Id],[RoleId], [ClaimType], [ClaimValue])
	)

-- Reference Data for Role Claim
MERGE INTO dbo.[ROLECLAIM] AS [Target]
USING RoleClaim_CTE AS [Source]
	ON [Target].[RoleId] = [Source].[RoleId] and [Target].[ClaimValue] = [Source].[ClaimValue]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
		[Id],
			  [RoleId]
			, [ClaimType]
			, [ClaimValue]
			)
		VALUES (
			[Source].[Id],
			  [Source].[RoleId]
			, [Source].[ClaimType]
			, [Source].[ClaimValue]
			)
WHEN MATCHED 
	THEN
		UPDATE SET [Target].[ClaimType] = [Source].[ClaimType]
	OUTPUT $action, inserted.*, deleted.*;
