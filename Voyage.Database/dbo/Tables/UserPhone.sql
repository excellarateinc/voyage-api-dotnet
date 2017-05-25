CREATE TABLE [dbo].[UserPhone] (
    [Id]          uniqueidentifier NOT NULL,
    [UserId]     uniqueidentifier NOT NULL,
    [PhoneNumber] NVARCHAR (15)  NOT NULL,
    [PhoneType]   INT            NOT NULL,
    [VerificationCode] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_dbo.UserPhones] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserPhones_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[UserPhone]([UserId] ASC);

