CREATE TABLE [dbo].[ApplicationLog] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Message]         NVARCHAR (MAX) NULL,
    [MessageTemplate] NVARCHAR (MAX) NULL,
    [Level]           NVARCHAR (128) NULL,
    [TimeStamp]       DATETIME       NOT NULL,
    [Exception]       NVARCHAR (MAX) NULL,
    [LogEvent]        NVARCHAR (MAX) NULL,
    [Properties]      XML            NULL,
    CONSTRAINT [PK_dbo.LaunchpadLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

