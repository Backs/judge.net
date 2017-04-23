CREATE TABLE [dbo].[ContestTasks]
(
    [ContestId] INT NOT NULL,
    [TaskId] BIGINT NOT NULL,
    [TaskName] nvarchar(5) NOT NULL,
    CONSTRAINT [PK_ContestTasks] PRIMARY KEY CLUSTERED ([ContestId], [TaskId]),
    CONSTRAINT [FK_ContestTasks_Contests] FOREIGN KEY ([ContestId]) REFERENCES [dbo].[Contests] ([Id]),
    CONSTRAINT [FK_ContestTasks_Tasks] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id])
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_ContestTasks_ContestId_TaskName] 
    ON [dbo].[ContestTasks] (ContestId, TaskName)