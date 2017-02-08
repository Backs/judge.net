CREATE TABLE [dbo].[Tasks]
(
    [Id] BIGINT NOT NULL PRIMARY KEY, 
    [TestsFolder] NVARCHAR(512) NOT NULL, 
    [TimeLimitMilliseconds] INT NOT NULL, 
    [MemoryLimitBytes] INT NOT NULL
)
