CREATE TABLE [dbo].[Tasks]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1), 
    [TestsFolder] NVARCHAR(512) NOT NULL, 
    [TimeLimitMilliseconds] INT NOT NULL, 
    [MemoryLimitBytes] INT NOT NULL, 
    [Name] NVARCHAR(256) NOT NULL,
    [CreationDateUtc] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    [Statement] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([Id] ASC)
)

