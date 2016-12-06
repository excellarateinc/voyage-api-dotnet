CREATE TABLE [dbo].[UserLogin] (
    [LoginProvider]      NVARCHAR (128) NOT NULL,
    [ProviderKey]        NVARCHAR (128) NOT NULL,
    [UserId]             NVARCHAR (128) NOT NULL,
    [ApplicationUser_Id] NVARCHAR (128) NULL,
    CONSTRAINT [PK_core.UserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_core.UserLogins_core.Users_ApplicationUser_Id] FOREIGN KEY ([ApplicationUser_Id]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationUser_Id]
    ON [dbo].[UserLogin]([ApplicationUser_Id] ASC);

