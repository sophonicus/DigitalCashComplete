CREATE TABLE [dbo].[CustomerAccount]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(MAX) NULL, 
    [Balance] DECIMAL(18, 2) NULL
)
