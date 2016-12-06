WITH UserPhone_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			 (N'fb9f65d2-699c-4f08-a2e4-8e6c28190a84', N'5555555555', 2)
		) AS PhoneSeed(UserId, Phone, PhoneType)
	)
-- Reference Data for Role 
MERGE INTO dbo.[UserPhone] AS [Target]
USING UserPhone_CTE AS [Source]
	ON [Target].[UserId] = [Source].[UserId]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			[UserId],
			[PhoneNumber],
			[PhoneType]
			)
		VALUES (
			 Source.[UserId]
			,Source.[Phone]
			,Source.[PhoneType]
			)
		OUTPUT $action, inserted.*, deleted.*;
