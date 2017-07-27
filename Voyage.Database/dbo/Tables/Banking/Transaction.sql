CREATE TABLE [dbo].[Transaction]
(
	[TransactionId] INT NOT NULL PRIMARY KEY IDENTITY (1, 1), 
    [AccountId] INT NOT NULL, 
	[Date] DATETIME2 NOT NULL, 
    [Type] INT NOT NULL,
	[Description] NVARCHAR(128) NOT NULL,
	[Amount] DECIMAL(18, 2) NOT NULL,
	[Balance] DECIMAL(18, 2) NOT NULL,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETDATE()
)
