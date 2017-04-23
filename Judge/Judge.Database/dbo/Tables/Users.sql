CREATE TABLE [dbo].[Users] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserName]     NVARCHAR (256) NOT NULL,
    [PasswordHash] NVARCHAR (256) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL, 
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Users_Email] ON [dbo].[Users] (Email)

GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_Users_UserName] ON [dbo].[Users] (UserName)
