CREATE TABLE [dbo].[CheckQueue]
(
    [SubmitResultId] BIGINT NOT NULL, 
    [CreationDateUtc] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [PK_CheckQueue] PRIMARY KEY ([SubmitResultId]),
    CONSTRAINT [FK_CheckQueue_SubmitResults] FOREIGN KEY ([SubmitResultId]) REFERENCES dbo.SubmitResults([Id])
)
