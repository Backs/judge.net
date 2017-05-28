CREATE TABLE [dbo].[Submits]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1), 
    [UserId] BIGINT NOT NULL, 
    [ProblemId] BIGINT NOT NULL,
    [ContestId] INT NULL,
    [LanguageId] INT NOT NULL, 
    [FileName] NVARCHAR(256) NOT NULL, 
    [SourceCode] NVARCHAR(MAX) NOT NULL, 
    [SubmitDateUtc] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
    [SubmitType] TINYINT NOT NULL DEFAULT(1),
    CONSTRAINT [PK_Submits] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Submits_Users] FOREIGN KEY (UserId) REFERENCES dbo.Users(Id),
    CONSTRAINT [FK_Submits_Languages] FOREIGN KEY (LanguageId) REFERENCES dbo.Languages(Id),
    CONSTRAINT [CK_SubmitType] CHECK ([SubmitType] = 1 OR [SubmitType] = 2),
    CONSTRAINT [CK_Submits_Contest] CHECK ([SubmitType] = 2 AND [ContestId] IS NOT NULL OR [SubmitType] = 1),
    CONSTRAINT [FK_Submits_Contests] FOREIGN KEY (ContestId) REFERENCES dbo.Contests(Id)
)
GO
CREATE NONCLUSTERED INDEX [IX_Submits_ProblemId_UserId] 
ON [dbo].[Submits] ([ProblemId], [UserId]) WITH (ONLINE = ON)