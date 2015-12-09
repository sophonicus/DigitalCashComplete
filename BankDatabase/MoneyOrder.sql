CREATE TABLE [dbo].[MoneyOrder]
(
	[Id] INT NOT NULL IDENTITY(0,1) PRIMARY KEY, 
    [UniqueId] VARCHAR(MAX) NOT NULL, 
    [Amount] DECIMAL(18, 2) NULL, 
    [Signed] BIT NOT NULL DEFAULT 0, 
    [Signature] VARCHAR(MAX) NULL, 
    [BlindSignature] VARCHAR(MAX) NOT NULL, 
    [Cashed] BIT NOT NULL DEFAULT 0, 
    [CashRequest] BIT NOT NULL DEFAULT 0
)
