CREATE TABLE [dbo].[ClientScopeType]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(500) NOT NULL, 
    [Description] NVARCHAR(1000) NOT NULL,
	[CreatedBy] nvarchar(255) NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[LastModifiedBy] nvarchar(255) NOT NULL,
	[LastModifiedDate] datetime NOT NULL, 
    [IsDeleted] BIT NOT NULL

)
