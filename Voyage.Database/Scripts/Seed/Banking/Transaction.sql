WITH Transaction_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			  (1, 1, GETDATE(), 0, 'ACH Payment', 50.00, 950.00, GETDATE())
			 ,(2, 2, GETDATE(), 1, 'Paycheck', 250.00, 4250.00, GETDATE())
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
WHEN NOT MATCHED BY SOURCE
	THEN DELETE
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
