WITH UserPhone_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			 (CONVERT(uniqueidentifier, 'ae0a11dc-9d49-490d-8859-168532541100'),CONVERT(uniqueidentifier, 'fb9f65d2-699c-4f08-a2e4-8e6c28190a84'), N'5555555555', 2)
		) AS PhoneSeed([Id],[UserId], [Phone], [PhoneType])
	)
-- Reference Data for User Phone 
MERGE INTO dbo.[UserPhone] AS [Target]
USING UserPhone_CTE AS [Source]
	ON [Target].[UserId] = [Source].[UserId]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
		[Id],
			[UserId],
			[PhoneNumber],
			[PhoneType]
			)
		VALUES (
		[Source].[Id]
			,  [Source].[UserId]
			, [Source].[Phone]
			, [Source].[PhoneType]
			)
		OUTPUT $action, inserted.*, deleted.*;
