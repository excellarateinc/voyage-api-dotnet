WITH Transaction_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			  (1, 1, GETDATE(), 0, 'description', 50.00, 1100.34, GETDATE())
			 ,(2, 1, GETDATE(), 1, 'other thingy', 25.00, 4433.00, GETDATE())
		) AS TransactionSeed([TransactionId], [AccountId], [Date], [Type], [Description], [Amount], [Balance], [CreatedDate])
	)

-- Reference Data for Transaction
MERGE INTO dbo.[Transaction] AS [Target]
USING Transaction_CTE AS [Source]
	ON [Target].[TransactionId] = [Source].[TransactionId]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			 [AccountId] 
			,[Date]
			,[Type]
			,[Description]
			,[Amount]
			,[Balance]
			,[CreatedDate] 
			)
		VALUES (
		    [Source].[AccountId]
		   ,[Source].[Date]
		   ,[Source].[Type]
		   ,[Source].[Description]
		   ,[Source].[Amount]
		   ,[Source].[Balance]
		   ,[Source].[CreatedDate]
			)
WHEN MATCHED 
	THEN
		UPDATE SET 	
			[Target].[AccountId] = [Source].[AccountId]
		   ,[Target].[Date] = [Source].[Date]
		   ,[Target].[Type] = [Source].[Type]
		   ,[Target].[Description] = [Source].[Description]
		   ,[Target].[Amount] = [Source].[Amount]
		   ,[Target].[Balance] = [Source].[Balance]
		   ,[Target].[CreatedDate] = [Source].[CreatedDate]
	OUTPUT $action, inserted.*, deleted.*;
