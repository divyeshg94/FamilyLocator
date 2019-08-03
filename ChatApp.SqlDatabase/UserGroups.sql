CREATE TABLE [dbo].[UserGroups]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [GroupId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    CONSTRAINT [FK_UserGroups_Groups] FOREIGN KEY (GroupId) REFERENCES [Groups]([Id]),
    CONSTRAINT [FK_UserGroups_Users] FOREIGN KEY (UserId) REFERENCES [Users]([Id])
)
