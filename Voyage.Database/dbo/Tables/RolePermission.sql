CREATE TABLE [dbo].[RolePermission] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RoleId]     NVARCHAR (128) NOT NULL,
    [PermissionType]  NVARCHAR (MAX) NULL,
    [PermissionValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.RolePermission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.RolePermission_dbo.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[RolePermission]([RoleId] ASC);

