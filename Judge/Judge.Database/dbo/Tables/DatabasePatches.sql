CREATE TABLE [dbo].[DatabasePatches]
(
    [Name] nvarchar(128) NOT NULL,
    [ApplyDate] datetime NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_DatabasePatches] PRIMARY KEY (Name)
)
