WITH ClientScope_CTE
AS (
	SELECT *
	FROM (
		VALUES (
			N'f000aeed-5158-441d-b704-b1503f6af711'
			,N'2885FDDD-9F03-48EB-8762-9BD176EBB496'
			,N'ef9281e3-18b3-4728-9ecd-0a1b0b4b783f'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
			,(
			N'914d66b5-8451-45f3-983f-51c1749cbc17'
			,N'2885FDDD-9F03-48EB-8762-9BD176EBB496'
			,N'08901a74-c7e3-418e-925d-87b12c36e1f8'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
			,(
			N'bea85095-5468-4ee1-be35-ad645da07ad3'
			,N'2885FDDD-9F03-48EB-8762-9BD176EBB496'
			,N'ddf7b2c6-4537-4822-b09c-a9d2c3797f75'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
			,(
			N'fd6d0595-bd49-4fd9-9a1b-dea112bbbe0f'
			,N'26C092FC-64DB-4334-87B5-09428F025C9C'
			,N'ef9281e3-18b3-4728-9ecd-0a1b0b4b783f'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
			,(
			N'b31afdf5-969a-4768-bc9d-ef87bae2d13e'
			,N'26C092FC-64DB-4334-87B5-09428F025C9C'
			,N'08901a74-c7e3-418e-925d-87b12c36e1f8'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
			,(
			N'd1c1f470-ab60-49f6-9a23-1f2bd60cdf71'
			,N'26C092FC-64DB-4334-87B5-09428F025C9C'
			,N'ddf7b2c6-4537-4822-b09c-a9d2c3797f75'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,N'FirstName3@app.com'
			,'Jan 1 2017 1:29PM'
			,0)
		) AS ClientScopeSeed(
		[Id],
           [ClientId]
           ,[ClientScopeTypeId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[LastModifiedBy]
           ,[LastModifiedDate]
           ,[IsDeleted])
	)

--Merge 
MERGE INTO dbo.[ClientScope] AS [Target]
USING ClientScope_CTE AS [Source]
	ON [Target].[ClientId] = [Source].[ClientId] AND [Target].[ClientScopeTypeId] = [Source].[ClientScopeTypeId]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
		[Id],
			[ClientId]
           ,[ClientScopeTypeId]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[LastModifiedBy]
           ,[LastModifiedDate]
           ,[IsDeleted]
			)
		VALUES (
		[Source].[Id],
			[Source].[ClientId]
           ,[Source].[ClientScopeTypeId]
           ,[Source].[CreatedBy]
           ,[Source].[CreatedDate]
           ,[Source].[LastModifiedBy]
           ,[Source].[LastModifiedDate]
           ,[Source].[IsDeleted]
			)
		OUTPUT $action, inserted.*, deleted.*;