CREATE TABLE [dbo].[Submits]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1), 
    [UserId] BIGINT NOT NULL, 
    [ProblemId] BIGINT NOT NULL, 
    [LanguageId] INT NOT NULL, 
    [FileName] NVARCHAR(256) NOT NULL, 
    [SourceCode] NVARCHAR(MAX) NOT NULL, 
    [SubmitDateUtc] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Submits] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Submits_Users] FOREIGN KEY (UserId) REFERENCES dbo.Users(Id),
    CONSTRAINT [FK_Submits_Languages] FOREIGN KEY (LanguageId) REFERENCES dbo.Languages(Id)
)
GO
CREATE NONCLUSTERED INDEX [IX_Submits_ProblemId_UserId] 
ON [dbo].[Submits] ([ProblemId], [UserId]) WITH (ONLINE = ON)