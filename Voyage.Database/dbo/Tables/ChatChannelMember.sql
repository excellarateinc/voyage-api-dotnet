CREATE TABLE [dbo].[ChatChannelMember]
(
	[ChannelMemberId] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[ChannelId] INT NOT NULL,
	[UserId] NVARCHAR(128) NOT NULL,
	[CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_ChatChannelMember_Channel] FOREIGN KEY ([ChannelId]) REFERENCES [ChatChannel]([ChannelId]),
	CONSTRAINT [FK_ChatChannelMember_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
