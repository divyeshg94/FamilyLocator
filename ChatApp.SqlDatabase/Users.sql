CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] NVARCHAR(50) NOT NULL, 
    [EmailId] NVARCHAR(50) NULL, 
    [CreatedDate] DATETIME NOT NULL, 
    [IsExists] BIT NOT NULL DEFAULT 1
)
