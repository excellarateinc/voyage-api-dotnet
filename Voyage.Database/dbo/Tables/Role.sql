CREATE TABLE [dbo].[Role] (
    [Id]   uniqueidentifier NOT NULL,
    [Name] NVARCHAR (256) NOT NULL,
    [Description] NVARCHAR(256) NOT NULL DEFAULT '', 
    CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[Role]([Name] ASC);

