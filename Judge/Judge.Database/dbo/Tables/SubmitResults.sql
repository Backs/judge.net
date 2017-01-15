CREATE TABLE [dbo].[SubmitResults]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1), 
    [SubmitId] BIGINT NOT NULL, 
    [Status] INT NOT NULL, 
    [PassedTests] INT NULL,
    [TotalBytes] INT NULL,
    [TotalMilliseconds] INT NULL
    CONSTRAINT [PK_SubmitResults] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SubmitResults_Submits] FOREIGN KEY ([SubmitId]) REFERENCES [dbo].[Submits]([Id]),
    CONSTRAINT [CK_SubmitResults_Status] CHECK ([Status] = 0 OR [Status] = 1 OR [Status] = 2 OR [Status] = 3 OR [Status] = 4 OR [Status] = 5 OR [Status] = 6 OR [Status] = 7)
)
