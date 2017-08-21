CREATE TABLE [dbo].[ProfileImage]
(
	[ProfileImageId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [UserId] NVARCHAR (128) NOT NULL,
	[ImageData] NVARCHAR(MAX) NOT NULL,
	CONSTRAINT [FK_ProfileImage_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[ProfileImage]([UserId] ASC);