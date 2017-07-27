WITH Account_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			  (1, 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', '0023287854', 'My Checking Acct', 0, 1450.25, GETDATE())
			 ,(2, 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', '2308287468', 'Personal Savings Acct', 1, 4520.88, GETDATE())
		) AS AccountSeed([AccountId], [UserId], [AccountNumber], [Name], [Type], [Balance], [CreatedDate])
	)

-- Reference Data for Account
MERGE INTO dbo.[Account] AS [Target]
USING Account_CTE AS [Source]
	ON [Target].[AccountId] = [Source].[AccountId]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			 [UserId]
			,[AccountNumber]
			,[Name]
			,[Type]
			,[Balance]
			,[CreatedDate] 
			)
		VALUES (
		    [Source].[UserId]
		   ,[Source].[AccountNumber]
		   ,[Source].[Name]
		   ,[Source].[Type]
		   ,[Source].[Balance]
		   ,[Source].[CreatedDate]
			)
WHEN MATCHED 
	THEN
		UPDATE SET 	
			[Target].[UserId] = [Source].[UserId]
		   ,[Target].[AccountNumber] = [Source].[AccountNumber]
		   ,[Target].[Name] = [Source].[Name]
		   ,[Target].[Type] = [Source].[Type]
		   ,[Target].[Balance] = [Source].[Balance]
		   ,[Target].[CreatedDate] = [Source].[CreatedDate]
	OUTPUT $action, inserted.*, deleted.*;
