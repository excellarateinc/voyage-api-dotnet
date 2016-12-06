WITH Widget_CTE 
AS (
	SELECT *
	FROM (
		VALUES 
			(N'Seed Widget', N'Green')
		) AS UserRole(Name, Color)
	)
-- Reference Data for Role 
MERGE INTO dbo.[Widget] AS [Target]
USING Widget_CTE AS [Source]
	ON [Target].[Name] = [Source].[Name]
WHEN NOT MATCHED BY TARGET
	THEN
		INSERT (
			[Name], [Color]
			)
		VALUES (
			Source.Name,
			Source.Color
			)
		OUTPUT $action, inserted.*, deleted.*;
