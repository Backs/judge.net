CREATE TABLE [dbo].[CheckQueue]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1), 
    [SubmitId] BIGINT NOT NULL, 
    [CreationDateUtc] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [PK_CheckQueue] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CheckQueue_Submits] FOREIGN KEY ([SubmitId]) REFERENCES dbo.Submits([Id])
)
