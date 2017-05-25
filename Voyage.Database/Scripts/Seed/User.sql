 WITH User_CTE
AS (
	SELECT *
	FROM (
		VALUES (
			CONVERT(uniqueidentifier, 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84')
			,N'Admin_First'
			,N'Admin_Last'
			,1
			,N'admin@admin.com'
			,0
			,N'AGm314lswtDFuAaMlptMbX1xCzz9eF6fIRXuMDFqkhoCV72AqdYEBXQ9HODk608uCA=='
			,N'cb31329f-bb59-4803-a1c7-67adc3dc8871'
			,NULL
			,0
			,0
			,NULL
			,0
			,0
			,N'admin@admin.com'
			,0
			)
		) AS UserSeed([Id], [FirstName], [LastName], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Deleted])
	)

--Merge 
MERGE INTO dbo.[User] AS [Target]
USING User_CTE AS [Source]
	ON [Target].[Id] = [Source].[Id]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			[Id]
			,[FirstName]
			,[LastName]
			,[IsActive]
			,[Email]
			,[EmailConfirmed]
			,[PasswordHash]
			,[SecurityStamp]
			,[PhoneNumber]
			,[PhoneNumberConfirmed]
			,[TwoFactorEnabled]
			,[LockoutEndDateUtc]
			,[LockoutEnabled]
			,[AccessFailedCount]
			,[UserName]
			,[Deleted]
			)
		VALUES (
			  [Source].[Id]
			, [Source].[FirstName]
			, [Source].[LastName]
			, [Source].[IsActive]
			, [Source].[Email]
			, [Source].[EmailConfirmed]
			, [Source].[PasswordHash]
			, [Source].[SecurityStamp]
			, [Source].[PhoneNumber]
			, [Source].[PhoneNumberConfirmed]
			, [Source].[TwoFactorEnabled]
			, [Source].[LockoutEndDateUtc]
			, [Source].[LockoutEnabled]
			, [Source].[AccessFailedCount]
			, [Source].[UserName]
			, [Source].[Deleted]
			)
		OUTPUT $action, inserted.*, deleted.*;