CREATE TABLE [dbo].[SubmitResults]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1), 
    [SubmitId] BIGINT NOT NULL, 
    [Status] INT NOT NULL, 
    [PassedTests] INT NULL,
    [TotalBytes] INT NULL,
    [TotalMilliseconds] INT NULL,
    [CompileOutput] NVARCHAR(MAX) NULL, 
    [RunDescription] NVARCHAR(MAX) NULL,
    [RunOutput] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_SubmitResults] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SubmitResults_Submits] FOREIGN KEY ([SubmitId]) REFERENCES [dbo].[Submits]([Id]),
)
