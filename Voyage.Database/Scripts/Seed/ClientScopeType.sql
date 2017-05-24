WITH ClientScopeType_CTE
AS (
	SELECT *
	FROM (
		VALUES (CONVERT(uniqueidentifier, 'ef9281e3-18b3-4728-9ecd-0a1b0b4b783f')
			,N'Profile'
			,N'Profile SCope'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
			,(CONVERT(uniqueidentifier, '08901a74-c7e3-418e-925d-87b12c36e1f8')
			,N'Email'
			,N'Email SCope'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
			,(
			CONVERT(uniqueidentifier, 'ddf7b2c6-4537-4822-b09c-a9d2c3797f75')
			,N'Api'
			,N'Api SCope'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
		) AS ClientScopeTypeSeed(
			[Id]
			,[Name]
			,[Description]
			,[CreatedBy]
			,[CreatedDate]
			,[LastModifiedBy]
			,[LastModifiedDate]
			,[IsDeleted])
	)

--Merge 
MERGE INTO dbo.[ClientScopeType] AS [Target]
USING ClientScopeType_CTE AS [Source]
	ON [Target].[Id] = [Source].[Id]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT ([Id]
			,[Name]
			,[Description]
			,[CreatedBy]
			,[CreatedDate]
			,[LastModifiedBy]
			,[LastModifiedDate]
			,[IsDeleted]
			)
		VALUES (
			[Source].[Id]
			,[Source].[Name]
			,[Source].[Description]
			,[Source].[CreatedBy]
			,[Source].[CreatedDate]
			,[Source].[LastModifiedBy]
			,[Source].[LastModifiedDate]
			,[Source].[IsDeleted]
			)
		OUTPUT $action, inserted.*, deleted.*;