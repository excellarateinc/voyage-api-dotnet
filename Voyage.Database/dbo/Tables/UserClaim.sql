CREATE TABLE [dbo].[UserClaim] (
    [Id]                 uniqueidentifier NOT NULL,
    [UserId]             NVARCHAR (MAX) NULL,
    [ClaimType]          NVARCHAR (MAX) NULL,
    [ClaimValue]         NVARCHAR (MAX) NULL,
    [ApplicationUser_Id] uniqueidentifier NULL,
    CONSTRAINT [PK_dbo.UserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserClaims_dbo.Users_ApplicationUser_Id] FOREIGN KEY ([ApplicationUser_Id]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationUser_Id]
    ON [dbo].[UserClaim]([ApplicationUser_Id] ASC);

