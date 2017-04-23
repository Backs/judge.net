CREATE TABLE [dbo].[Contests]
(
    [Id] INT NOT NULL IDENTITY(1,1), 
    [Name] NVARCHAR(128) NOT NULL, 
    [StartTime] DATETIME NOT NULL, 
    [FinishTime] DATETIME NOT NULL, 
    [FreezeTime] DATETIME NULL,
    [IsOpened] BIT,
    CONSTRAINT [PK_Contests] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Contests_FreezeTime] CHECK ([FreezeTime] IS NULL OR ([FreezeTime] > [StartTime] AND [FreezeTime] < [FinishTime]))
)