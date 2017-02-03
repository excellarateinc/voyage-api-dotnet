CREATE TABLE [dbo].[LogMetadata] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [AuditLogId] BIGINT         NOT NULL,
    [Key]        NVARCHAR (MAX) NULL,
    [Value]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.LogMetadata] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.LogMetadata_dbo.AuditLog_AuditLogId] FOREIGN KEY ([AuditLogId]) REFERENCES [dbo].[AuditLog] ([AuditLogId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AuditLogId]
    ON [dbo].[LogMetadata]([AuditLogId] ASC);

