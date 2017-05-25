CREATE TABLE [dbo].[ClientScope]
(
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL, 
	[ClientId] UNIQUEIDENTIFIER NOT NULL,
    [ClientScopeTypeId] UNIQUEIDENTIFIER NOT NULL,
	[CreatedBy] nvarchar(255) NOT NULL,
	[CreatedDate] datetime NOT NULL,
	[LastModifiedBy] nvarchar(255) NOT NULL,
	[LastModifiedDate] datetime NOT NULL, 
    [IsDeleted] BIT NOT NULL,
	CONSTRAINT [FK_dbo.ClientScopes_dbo.Clients_Id] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Client] ([Id]),
	CONSTRAINT [FK_dbo.ClientScopes_dbo.Client_Scope_Type_Id] FOREIGN KEY ([ClientScopeTypeId]) REFERENCES [dbo].[ClientScopeType] ([Id]) 
);

GO
CREATE NONCLUSTERED INDEX [IX_ClientScope_Client_Id]
    ON [dbo].[ClientScope]([ClientId] ASC);