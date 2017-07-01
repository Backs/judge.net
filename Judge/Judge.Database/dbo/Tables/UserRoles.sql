CREATE TABLE [dbo].[UserRoles]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1),
    [UserId] BIGINT NOT NULL,
    [RoleName] NVARCHAR(32) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserRoles_UserId] FOREIGN KEY (UserId) REFERENCES [dbo].[Users]([Id])
)
