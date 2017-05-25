CREATE TABLE [dbo].[ApplicationLog] (
    [Id]              UNIQUEIDENTIFIER  NOT NULL,
    [Message]         NVARCHAR (MAX) NULL,
    [MessageTemplate] NVARCHAR (MAX) NULL,
    [Level]           NVARCHAR (128) NULL,
    [TimeStamp]       DATETIME       NOT NULL,
    [Exception]       NVARCHAR (MAX) NULL,
    [LogEvent]        NVARCHAR (MAX) NULL,
    [Properties]      XML            NULL,
    CONSTRAINT [PK_dbo.VoyageLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

