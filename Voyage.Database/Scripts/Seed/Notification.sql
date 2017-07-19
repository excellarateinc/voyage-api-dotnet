WITH Notification_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			  (1, 'Test Notification', 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', 0, 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', GETDATE())
			 ,(2, 'Meeting Reminder for 2 PM', 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', 0, 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', GETDATE())
		) AS NotificationSeed([Id],[Text], [AssignedToUserId], [IsRead], [CreatedBy], [CreatedDate])
	)

-- Reference Data for Notification
MERGE INTO dbo.[Notification] AS [Target]
USING Notification_CTE AS [Source]
	ON [Target].[Id] = [Source].[Id]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
		    [Text]
		   ,[AssignedToUserId]
		   ,[IsRead]
		   ,[CreatedBy]
		   ,[CreatedDate]
			)
		VALUES (
		    [Source].[Text]
		   ,[Source].[AssignedToUserId]
		   ,[Source].[IsRead]
		   ,[Source].[CreatedBy]
		   ,[Source].[CreatedDate]
			)
WHEN MATCHED 
	THEN
		UPDATE SET 	
			[Target].[Text] = [Source].[Text]
		   ,[Target].[AssignedToUserId] = [Source].[AssignedToUserId]
		   ,[Target].[IsRead] = [Source].[IsRead]
		   ,[Target].[CreatedBy] = [Source].[CreatedBy]
		   ,[Target].[CreatedDate] = [Source].[CreatedDate]
	OUTPUT $action, inserted.*, deleted.*;
