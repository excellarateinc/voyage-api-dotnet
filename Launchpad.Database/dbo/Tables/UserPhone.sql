CREATE TABLE [dbo].[UserPhone] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (128) NOT NULL,
    [PhoneNumber] NVARCHAR (15)  NOT NULL,
    [PhoneType]   INT            NOT NULL,
    CONSTRAINT [PK_core.UserPhones] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_core.UserPhones_core.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[UserPhone]([UserId] ASC);

