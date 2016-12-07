CREATE TABLE [dbo].[Widget] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (60) NULL,
    [Color] NVARCHAR (60) NULL,
    CONSTRAINT [PK_dbo.Widget] PRIMARY KEY CLUSTERED ([Id] ASC)
);

