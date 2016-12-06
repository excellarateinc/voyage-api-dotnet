CREATE TABLE [dbo].[ActivityAudit] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RequestId]  NVARCHAR (64)  NULL,
    [Method]     NVARCHAR (32)  NULL,
    [Path]       NVARCHAR (128) NULL,
    [IpAddress]  NVARCHAR (64)  NULL,
    [Date]       DATETIME       NOT NULL,
    [StatusCode] INT            NOT NULL,
    [Error]      NVARCHAR (MAX) NULL,
    [UserName]   NVARCHAR (50)  NULL,
    CONSTRAINT [PK_core.ActivityAudit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

