WITH Notification_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			  (1, 'Test Notification', 'This is a test for notifications', 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', 0, 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', GETDATE())
			 ,(2, 'Meeting Coming Up', 'Meeting Reminder for 2 PM. Meeting with potential client about a potential big deal.', 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', 0, 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', GETDATE())
		) AS NotificationSeed([Id], [Subject], [Description], [AssignedToUserId], [IsRead], [CreatedBy], [CreatedDate])
	)

-- Reference Data for Notification
MERGE INTO dbo.[Notification] AS [Target]
USING Notification_CTE AS [Source]
	ON [Target].[Id] = [Source].[Id]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			[Subject],
		    [Description]
		   ,[AssignedToUserId]
		   ,[IsRead]
		   ,[CreatedBy]
		   ,[CreatedDate]
			)
		VALUES (
			[Source].[Subject]
		   ,[Source].[Description]
		   ,[Source].[AssignedToUserId]
		   ,[Source].[IsRead]
		   ,[Source].[CreatedBy]
		   ,[Source].[CreatedDate]
			)
WHEN MATCHED 
	THEN
		UPDATE SET 	
			[Target].[Subject] = [Source].[Subject]
		   ,[Target].[Description] = [Source].[Description]
		   ,[Target].[AssignedToUserId] = [Source].[AssignedToUserId]
		   ,[Target].[IsRead] = [Source].[IsRead]
		   ,[Target].[CreatedBy] = [Source].[CreatedBy]
		   ,[Target].[CreatedDate] = [Source].[CreatedDate]
	OUTPUT $action, inserted.*, deleted.*;
