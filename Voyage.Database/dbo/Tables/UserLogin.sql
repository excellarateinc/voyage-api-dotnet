CREATE TABLE [dbo].[UserLogin] (
    [LoginProvider]      NVARCHAR (128) NOT NULL,
    [ProviderKey]        NVARCHAR (128) NOT NULL,
    [UserId]             NVARCHAR (128) NOT NULL,
    [ApplicationUser_Id] uniqueidentifier NULL,
    CONSTRAINT [PK_dbo.UserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.UserLogins_dbo.Users_ApplicationUser_Id] FOREIGN KEY ([ApplicationUser_Id]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationUser_Id]
    ON [dbo].[UserLogin]([ApplicationUser_Id] ASC);

