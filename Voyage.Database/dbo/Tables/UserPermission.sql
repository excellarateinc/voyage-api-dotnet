CREATE TABLE [dbo].[UserPermission] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [UserId]             NVARCHAR (MAX) NULL,
    [PermissionType]          NVARCHAR (MAX) NULL,
    [PermissionValue]         NVARCHAR (MAX) NULL,
    [ApplicationUser_Id] NVARCHAR (128) NULL,
    CONSTRAINT [PK_dbo.UserPermission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserPermission_dbo.Users_ApplicationUser_Id] FOREIGN KEY ([ApplicationUser_Id]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationUser_Id]
    ON [dbo].[UserPermission]([ApplicationUser_Id] ASC);

