WITH Client_CTE
AS (
	SELECT *
	FROM (
		VALUES (
			N'2885FDDD-9F03-48EB-8762-9BD176EBB496'
			,N'Client1'
			,N'123456'
			,N'abcdef'
			,1
			,1
			,1
			,86400
			,86400
			,NULL
			,NULL
			,1
			,0
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
			,(N'26C092FC-64DB-4334-87B5-09428F025C9C'
			,N'Client2'
			,N'client-super'
			,N'secret'
			,1
			,1
			,1
			,86400
			,86400
			,NULL
			,NULL
			,1
			,0
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
		) AS ClientSeed(
			[Id]
           ,[Name]
           ,[ClinetIdentifier]
           ,[ClientSecret]
           ,[IsSecretRequired]
           ,[IsScoped]
           ,[IsAutoApprove]
           ,[AccessTokenValiditySeconds]
           ,[RefreshTokenValiditySeconds]
           ,[FailedLoginAttempts]
           ,[ForceTokenExpiredate]
           ,[IsEnabled]
           ,[IsAccountLocked]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[LastModifiedBy]
           ,[LastModifiedDate]
           ,[IsDeleted])
	)

--Merge 
MERGE INTO dbo.[Client] AS [Target]
USING Client_CTE AS [Source]
	ON [Target].[Name] = [Source].[Name]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			[Id]
           ,[Name]
           ,[ClinetIdentifier]
           ,[ClientSecret]
           ,[IsSecretRequired]
           ,[IsScoped]
           ,[IsAutoApprove]
           ,[AccessTokenValiditySeconds]
           ,[RefreshTokenValiditySeconds]
           ,[FailedLoginAttempts]
           ,[ForceTokenExpiredate]
           ,[IsEnabled]
           ,[IsAccountLocked]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[LastModifiedBy]
           ,[LastModifiedDate]
           ,[IsDeleted]
			)
		VALUES (
			 [Source].[Id]
           ,[Source].[Name]
           ,[Source].[ClinetIdentifier]
           ,[Source].[ClientSecret]
           ,[Source].[IsSecretRequired]
           ,[Source].[IsScoped]
           ,[Source].[IsAutoApprove]
           ,[Source].[AccessTokenValiditySeconds]
           ,[Source].[RefreshTokenValiditySeconds]
           ,[Source].[FailedLoginAttempts]
           ,[Source].[ForceTokenExpiredate]
           ,[Source].[IsEnabled]
           ,[Source].[IsAccountLocked]
           ,[Source].[CreatedBy]
           ,[Source].[CreatedDate]
           ,[Source].[LastModifiedBy]
           ,[Source].[LastModifiedDate]
           ,[Source].[IsDeleted]
			)
		OUTPUT $action, inserted.*, deleted.*;