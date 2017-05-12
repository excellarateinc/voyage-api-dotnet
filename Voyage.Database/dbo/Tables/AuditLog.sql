CREATE TABLE [dbo].[AuditLog] (
    [AuditLogId]   uniqueidentifier NOT NULL,
    [UserName]     NVARCHAR (MAX) NULL,
    [EventDateUTC] DATETIME       NOT NULL,
    [EventType]    INT            NOT NULL,
    [TypeFullName] NVARCHAR (512) NOT NULL,
    [RecordId]     NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AuditLog] PRIMARY KEY CLUSTERED ([AuditLogId] ASC)
);

