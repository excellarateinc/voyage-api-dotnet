WITH UserRole_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			 (N'fb9f65d2-699c-4f08-a2e4-8e6c28190a84',	N'1cd39193-1f83-4d44-ab45-68c85be2acc8')
			,(N'fb9f65d2-699c-4f08-a2e4-8e6c28190a84',	N'927fe4a9-4e27-4635-a208-eb5afc953294')
			,(N'7E335050-E14D-4769-B2E1-E604CBEA69F2',	N'149DF98B-8B2D-4AE6-AD3B-7A3EF3C7CF0E')
		) AS UserRole([UserId], [RoleId])
	)
-- Reference Data for Role 
MERGE INTO dbo.[UserRole] AS [Target]
USING UserRole_CTE AS [Source]
	ON [Target].[UserId] = [Source].[UserId] and [Target].[RoleId] = [Source].[RoleId]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			[UserId],[RoleId],[ApplicationUser_Id]
			)
		VALUES (
			 [Source].[UserId]
			,[Source].[RoleId]
			,[Source].[UserId]
			)
		OUTPUT $action, inserted.*, deleted.*;
