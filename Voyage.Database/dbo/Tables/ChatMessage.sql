CREATE TABLE [dbo].[ChatMessage]
(
	[MessageId] INT NOT NULL PRIMARY KEY IDENTITY (1, 1), 
    [ChannelId] INT NOT NULL, 
	[Message] NVARCHAR(1000) NOT NULL,
	[Username] NVARCHAR(MAX) NOT NULL,
	[CreatedBy] NVARCHAR(128) NOT NULL, 
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
	CONSTRAINT [FK_ChatMessage_ChatChannel] FOREIGN KEY ([ChannelId]) REFERENCES [ChatChannel]([ChannelId]),
	CONSTRAINT [FK_ChatMessage_User] FOREIGN KEY ([CreatedBy]) REFERENCES [User]([Id])
)
