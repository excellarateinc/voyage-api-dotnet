﻿CREATE TABLE [dbo].[AuditLogDetail] (
    [Id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [PropertyName]  NVARCHAR (256) NOT NULL,
    [OriginalValue] NVARCHAR (MAX) NULL,
    [NewValue]      NVARCHAR (MAX) NULL,
    [AuditLogId]    BIGINT         NOT NULL,
    CONSTRAINT [PK_dbo.AuditLogDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AuditLogDetail_dbo.AuditLog_AuditLogId] FOREIGN KEY ([AuditLogId]) REFERENCES [dbo].[AuditLog] ([AuditLogId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AuditLogId]
    ON [dbo].[AuditLogDetail]([AuditLogId] ASC);

