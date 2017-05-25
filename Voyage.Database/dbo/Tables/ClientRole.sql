CREATE TABLE [dbo].[ClientRole]
(
	[ClientId] nvarchar(128) NOT NULL, 
    [RoleId] nvarchar(128) NOT NULL,
	CONSTRAINT [PK_dbo.ClientRoles] PRIMARY KEY CLUSTERED ([ClientId] ASC, [RoleId] ASC),
	CONSTRAINT [FK_dbo.ClientRoles_dbo.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ClientRoles_dbo.Clients_Id] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Client] ([Id])
);

GO
CREATE NONCLUSTERED INDEX [IX_ClientRole_Client_Id]
    ON [dbo].[ClientRole]([ClientId] ASC);
