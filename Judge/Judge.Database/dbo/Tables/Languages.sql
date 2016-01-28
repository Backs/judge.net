CREATE TABLE [dbo].[Languages] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (128) NOT NULL,
    [Description]         NVARCHAR (MAX) NULL,
    [IsCompilable]        BIT            CONSTRAINT [DF_Languages_IsCompilable] DEFAULT ((1)) NOT NULL,
    [CompileStringFormat] NVARCHAR (MAX) NULL,
    [RunStringFormat]     NVARCHAR (MAX) NOT NULL,
    [IsHidden]            BIT            CONSTRAINT [DF_Languages_IsHidden] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED ([Id] ASC)
);

