CREATE TABLE [dbo].[UserRole] (
    [UserId]             uniqueidentifier NOT NULL,
    [RoleId]             uniqueidentifier NOT NULL,
    [ApplicationUser_Id] uniqueidentifier NULL,
    CONSTRAINT [PK_dbo.UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.UserRoles_dbo.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.UserRoles_dbo.Users_ApplicationUser_Id] FOREIGN KEY ([ApplicationUser_Id]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[UserRole]([RoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationUser_Id]
    ON [dbo].[UserRole]([ApplicationUser_Id] ASC);

