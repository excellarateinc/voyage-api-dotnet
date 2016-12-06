CREATE TABLE [dbo].[UserClaim] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [UserId]             NVARCHAR (MAX) NULL,
    [ClaimType]          NVARCHAR (MAX) NULL,
    [ClaimValue]         NVARCHAR (MAX) NULL,
    [ApplicationUser_Id] NVARCHAR (128) NULL,
    CONSTRAINT [PK_core.UserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_core.UserClaims_core.Users_ApplicationUser_Id] FOREIGN KEY ([ApplicationUser_Id]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationUser_Id]
    ON [dbo].[UserClaim]([ApplicationUser_Id] ASC);

