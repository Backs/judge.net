CREATE TABLE [dbo].[Languages] (
    [Id]                        INT            IDENTITY (1, 1) NOT NULL,
    [Name]                      NVARCHAR (128) NOT NULL,
    [Description]               NVARCHAR (512) NULL,
    [IsCompilable]              BIT            CONSTRAINT [DF_Languages_IsCompilable] DEFAULT ((1)) NOT NULL,
    [CompilerPath]              NVARCHAR (512) NULL,
    [CompilerOptionsTemplate]   NVARCHAR (512) NULL,
    [OutputFileTemplate]        NVARCHAR (512) NULL,
    [RunStringFormat]           NVARCHAR (512) NOT NULL,
    [IsHidden]                  BIT            CONSTRAINT [DF_Languages_IsHidden] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED ([Id] ASC)
);

