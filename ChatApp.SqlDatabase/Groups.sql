﻿CREATE TABLE [dbo].[Groups]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [CreatedDate] DATETIME NOT NULL, 
    [IsExists] BIT NOT NULL DEFAULT 1
)