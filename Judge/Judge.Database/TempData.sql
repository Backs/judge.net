USE [Judge]
SET IDENTITY_INSERT [dbo].[Languages] ON
GO
INSERT [dbo].[Languages] ([Id], [Name], [Description], [IsCompilable], [CompilerPath], [CompilerOptionsTemplate], [OutputFileTemplate], [RunStringFormat], [IsHidden]) VALUES (1, N'C++', N'C++', 1, NULL, NULL, NULL, N'asd', 0)
GO
INSERT [dbo].[Languages] ([Id], [Name], [Description], [IsCompilable], [CompilerPath], [CompilerOptionsTemplate], [OutputFileTemplate], [RunStringFormat], [IsHidden]) VALUES (2, N'C#', N'C#', 1, N'C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe', N'{FileName}.{Ext}', N'{FileName}.exe', N'{FileName}.exe', 0)
GO
SET IDENTITY_INSERT [dbo].[Languages] OFF
GO
SET IDENTITY_INSERT [dbo].[Submits] ON 

GO
INSERT [dbo].[Submits] ([Id], [UserId], [ProblemId], [LanguageId], [FileName], [SourceCode], [SubmitDateUtc]) VALUES (37, 1, 1, 2, N'AB.cs', N'using System;

namespace Judge.Tests.TestSolutions
{
    class AB
    {
        public static void Main(string[] args)
        {
            var rows = Console.ReadLine().Split('' '');
            Console.WriteLine(int.Parse(rows[0]) + int.Parse(rows[1]));
        }
    }
}
', CAST(N'2017-02-12 13:50:07.883' AS DateTime))
GO
INSERT [dbo].[Submits] ([Id], [UserId], [ProblemId], [LanguageId], [FileName], [SourceCode], [SubmitDateUtc]) VALUES (38, 1, 1, 2, N'AB.cs', N'using System;

namespace Judge.Tests.TestSolutions
{
    class AB
    {
        public static void Main(string[] args)
        {
            var rows = Console.ReadLine().Split('' '');
            Console.WriteLine(int.Parse(rows[0]) + int.Parse(rows[1]));
        }
    }
}
', CAST(N'2017-02-12 14:38:26.263' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Submits] OFF
GO
SET IDENTITY_INSERT [dbo].[SubmitResults] ON 

GO
INSERT [dbo].[SubmitResults] ([Id], [SubmitId], [Status], [PassedTests], [TotalBytes], [TotalMilliseconds], [CompileOutput], [RunDescription], [RunOutput]) VALUES (19, 37, 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[SubmitResults] ([Id], [SubmitId], [Status], [PassedTests], [TotalBytes], [TotalMilliseconds], [CompileOutput], [RunDescription], [RunOutput]) VALUES (20, 38, 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[SubmitResults] OFF
GO
INSERT [dbo].[CheckQueue] ([SubmitResultId], [CreationDateUtc]) VALUES (19, CAST(N'2017-02-12 13:50:07.953' AS DateTime))
GO
INSERT [dbo].[CheckQueue] ([SubmitResultId], [CreationDateUtc]) VALUES (20, CAST(N'2017-02-12 14:38:26.273' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Tasks] ON 

GO
INSERT [dbo].[Tasks] ([Id], [TestsFolder], [TimeLimitMilliseconds], [MemoryLimitBytes], [Name], [CreationDateUtc], [Statement]) VALUES (1, N'AB', 1000, 104857600, N'A+B', CAST(N'2017-02-12 13:47:48.843' AS DateTime), N'**Statement**')
GO
SET IDENTITY_INSERT [dbo].[Tasks] OFF
GO
