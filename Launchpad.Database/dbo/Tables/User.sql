CREATE TABLE [dbo].[User] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [FirstName]            NVARCHAR (128) NOT NULL,
    [LastName]             NVARCHAR (128) NOT NULL,
    [IsActive]             BIT            NOT NULL,
    [Email]                NVARCHAR (MAX) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (MAX) NULL,
    [Deleted]              BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_core.Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

